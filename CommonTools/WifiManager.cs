using ManagedNativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class WifiManager
	{
		public static async Task<bool> IsConnectingAsync(string ssid = null)
		{
			return await Task.Run(async () =>
			{
				var connectedWifi = await GetConnectedWifiNameAysnc();
				if (String.IsNullOrWhiteSpace(ssid))
				{
					return !string.IsNullOrWhiteSpace(connectedWifi);
				}
				else
				{
					return !string.IsNullOrWhiteSpace(connectedWifi) && ssid == connectedWifi;
				}
			});
		}

		public static async Task<string> GetConnectedWifiNameAysnc()
		{
			try
			{
				var connectedWifi = await GetConnectedWifiAysnc();

				return connectedWifi.ToString();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task<NetworkIdentifier> GetConnectedWifiAysnc()
		{
			try
			{
				return await Task.Run(() => NativeWifi.EnumerateConnectedNetworkSsids().FirstOrDefault());
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task<AvailableNetworkPack> GetWifiAsync(string ssid, bool needToRefresh = true)
		{
			try
			{
				var wifiList = await GetWifiListAsync();
				if (wifiList.Any())
				{
					return wifiList.FirstOrDefault(w => w.Ssid.ToString() == ssid);
				}
				return null;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task<IEnumerable<string>> GetWifiNameListAsync(bool needToRefresh = true)
		{
			try
			{
				if (needToRefresh)
				{
					await RefreshWifiListAsync();
				}
				var wifiList = NativeWifi.EnumerateAvailableNetworks().Select(x => x.Ssid.ToString());
				return wifiList;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task<IEnumerable<AvailableNetworkPack>> GetWifiListAsync(bool needToRefresh = true)
		{
			try
			{
				if (needToRefresh)
				{
					await RefreshWifiListAsync();
				}
				var wifiList = NativeWifi.EnumerateAvailableNetworks().ToList();
				return wifiList;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task<bool> ConnectWifiAsync(string ssid)
		{
			try
			{
				var wifiToConnect = await GetWifiAsync(ssid);
				if (wifiToConnect == null)
				{
					throw new ArgumentException("Not found the wifi with specified ssid", nameof(ssid));
				}
				var result = await NativeWifi.ConnectNetworkAsync(wifiToConnect.Interface.Id, wifiToConnect.ProfileName, wifiToConnect.BssType, TimeSpan.FromSeconds(10));
				return result;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static async Task RefreshWifiListAsync(TimeSpan timeout = default)
		{
			try
			{
				await NativeWifi.ScanNetworksAsync(timeout == default ? TimeSpan.FromSeconds(10) : timeout);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
