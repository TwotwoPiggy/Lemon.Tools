using ManagedNativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools
{
    /// <summary>
    /// WiFi 网络管理器，提供 WiFi 连接和扫描功能。
    /// <para>
    /// 基于 ManagedNativeWifi 库实现，仅支持 Windows 平台。
    /// </para>
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class WifiManager
    {
        private static readonly TimeSpan DefaultScanTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan DefaultConnectTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// 检查是否已连接到 WiFi 网络。
        /// </summary>
        /// <param name="ssid">要检查的 SSID（可选，如果为空则检查是否有任何连接）。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>如果已连接返回 true，否则返回 false。</returns>
        public static async Task<bool> IsConnectedAsync(string? ssid = null, CancellationToken cancellationToken = default)
        {
            var connectedWifi = await GetConnectedWifiNameAsync(cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(ssid))
            {
                return !string.IsNullOrWhiteSpace(connectedWifi);
            }

            return string.Equals(connectedWifi, ssid, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取当前连接的 WiFi 名称。
        /// </summary>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>当前连接的 SSID，如果未连接则返回 null。</returns>
        public static async Task<string?> GetConnectedWifiNameAsync(CancellationToken cancellationToken = default)
        {
            var networkId = await GetConnectedWifiAsync(cancellationToken).ConfigureAwait(false);
            return networkId?.ToString();
        }

        /// <summary>
        /// 获取当前连接的 WiFi 网络标识符。
        /// </summary>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>当前连接的网络标识符，如果未连接则返回 null。</returns>
        public static async Task<NetworkIdentifier?> GetConnectedWifiAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Task.Run(() => NativeWifi.EnumerateConnectedNetworkSsids().FirstOrDefault(), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取指定 SSID 的 WiFi 网络信息。
        /// </summary>
        /// <param name="ssid">WiFi 网络的 SSID。</param>
        /// <param name="refreshFirst">是否先刷新网络列表。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>WiFi 网络信息，如果未找到则返回 null。</returns>
        public static async Task<AvailableNetworkPack?> GetWifiAsync(string ssid, bool refreshFirst = true, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ssid);

            var wifiList = await GetWifiListAsync(refreshFirst, cancellationToken).ConfigureAwait(false);
            return wifiList.FirstOrDefault(w => string.Equals(w.Ssid.ToString(), ssid, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取可用的 WiFi 网络名称列表。
        /// </summary>
        /// <param name="refreshFirst">是否先刷新网络列表。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>可用 WiFi 网络名称列表。</returns>
        public static async Task<IEnumerable<string>> GetWifiNameListAsync(bool refreshFirst = true, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (refreshFirst)
            {
                await RefreshWifiListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            return NativeWifi.EnumerateAvailableNetworks().Select(x => x.Ssid.ToString());
        }

        /// <summary>
        /// 获取可用的 WiFi 网络列表。
        /// </summary>
        /// <param name="refreshFirst">是否先刷新网络列表。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>可用 WiFi 网络列表。</returns>
        public static async Task<IEnumerable<AvailableNetworkPack>> GetWifiListAsync(bool refreshFirst = true, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (refreshFirst)
            {
                await RefreshWifiListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            return NativeWifi.EnumerateAvailableNetworks().ToList();
        }

        /// <summary>
        /// 连接到指定的 WiFi 网络。
        /// </summary>
        /// <param name="ssid">要连接的 WiFi 网络的 SSID。</param>
        /// <param name="timeout">连接超时时间。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>如果连接成功返回 true，否则返回 false。</returns>
        /// <exception cref="ArgumentException">指定的 WiFi 网络不存在。</exception>
        public static async Task<bool> ConnectWifiAsync(string ssid, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(ssid);

            var wifiToConnect = await GetWifiAsync(ssid, refreshFirst: true, cancellationToken).ConfigureAwait(false);

            if (wifiToConnect == null)
            {
                throw new ArgumentException($"WiFi network with SSID '{ssid}' not found.", nameof(ssid));
            }

            var connectTimeout = timeout ?? DefaultConnectTimeout;
            return await NativeWifi.ConnectNetworkAsync(
                wifiToConnect.InterfaceInfo.Id,
                wifiToConnect.ProfileName,
                wifiToConnect.BssType,
                connectTimeout).ConfigureAwait(false);
        }

        /// <summary>
        /// 刷新 WiFi 网络列表（扫描可用网络）。
        /// </summary>
        /// <param name="timeout">扫描超时时间。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        public static async Task RefreshWifiListAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var scanTimeout = timeout ?? DefaultScanTimeout;
            await NativeWifi.ScanNetworksAsync(scanTimeout).ConfigureAwait(false);
        }
    }
}
