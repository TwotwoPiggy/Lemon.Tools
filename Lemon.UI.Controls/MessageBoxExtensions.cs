using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Lemon.UI.Controls
{
    public static class TimedMessageBox
    {
        /// <summary>
        /// 显示带倒计时的消息框
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="caption">标题</param>
        /// <param name="timeoutSeconds">超时时间（秒）</param>
        /// <param name="buttons">按钮类型</param>
        /// <param name="icon">图标类型</param>
        /// <param name="defaultButton">默认按钮</param>
        /// <returns>对话框结果</returns>
        public static DialogResult Show(string text, string caption, int timeoutSeconds,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            using (var form = CreateForm(text, caption, buttons, icon, defaultButton))
            {
                // 设置倒计时
                var countdownLabel = form.Controls["countdownLabel"] as Label;
                var timer = new System.Windows.Forms.Timer { Interval = 1000 };
                int remaining = timeoutSeconds;

                timer.Tick += (sender, e) =>
                {
                    remaining--;
                    if (countdownLabel != null)
                    {
                        countdownLabel.Text = $"自动关闭倒计时: {remaining}秒";
                    }

                    if (remaining <= 0)
                    {
                        timer.Stop();
                        form.DialogResult = GetDefaultResult(buttons);
                        form.Close();
                    }
                };

                timer.Start();
                return form.ShowDialog();
            }
        }

        private static Form CreateForm(string text, string caption,
            MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton)
        {
            var form = new Form
            {
                Text = caption,
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // 消息标签
            var messageLabel = new Label
            {
                Name = "messageLabel",
                Text = text,
                Left = 20,
                Top = 20,
                Width = 360,
                Height = 60,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            // 倒计时标签
            var countdownLabel = new Label
            {
                Name = "countdownLabel",
                Text = "",
                Left = 20,
                Top = 90,
                Width = 360,
                Height = 20,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F,
                    System.Drawing.FontStyle.Italic)
            };

            // 按钮面板
            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Width = 360,
                Height = 30,
                Left = 20,
                Top = 120
            };

            // 根据按钮类型添加按钮
            AddButtons(buttonPanel, buttons, form);

            form.Controls.Add(messageLabel);
            form.Controls.Add(countdownLabel);
            form.Controls.Add(buttonPanel);

            return form;
        }

        private static void AddButtons(FlowLayoutPanel panel, MessageBoxButtons buttons, Form form)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    AddButton(panel, "确定", DialogResult.OK, form);
                    break;
                case MessageBoxButtons.OKCancel:
                    AddButton(panel, "取消", DialogResult.Cancel, form);
                    AddButton(panel, "确定", DialogResult.OK, form);
                    break;
                case MessageBoxButtons.YesNo:
                    AddButton(panel, "否", DialogResult.No, form);
                    AddButton(panel, "是", DialogResult.Yes, form);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    AddButton(panel, "取消", DialogResult.Cancel, form);
                    AddButton(panel, "否", DialogResult.No, form);
                    AddButton(panel, "是", DialogResult.Yes, form);
                    break;
                case MessageBoxButtons.RetryCancel:
                    AddButton(panel, "取消", DialogResult.Cancel, form);
                    AddButton(panel, "重试", DialogResult.Retry, form);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    AddButton(panel, "忽略", DialogResult.Ignore, form);
                    AddButton(panel, "重试", DialogResult.Retry, form);
                    AddButton(panel, "中止", DialogResult.Abort, form);
                    break;
            }
        }

        private static void AddButton(FlowLayoutPanel panel, string text,
            DialogResult result, Form form)
        {
            var button = new Button
            {
                Text = text,
                DialogResult = result,
                Width = 75,
                Height = 23,
                Margin = new Padding(3, 0, 0, 0)
            };

            button.Click += (sender, e) =>
            {
                form.DialogResult = result;
                form.Close();
            };

            panel.Controls.Add(button);
        }

        private static DialogResult GetDefaultResult(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                case MessageBoxButtons.OKCancel:
                    return DialogResult.OK;
                case MessageBoxButtons.YesNo:
                case MessageBoxButtons.YesNoCancel:
                    return DialogResult.Yes;
                case MessageBoxButtons.RetryCancel:
                    return DialogResult.Retry;
                case MessageBoxButtons.AbortRetryIgnore:
                    return DialogResult.Ignore;
                default:
                    return DialogResult.OK;
            }
        }
    }
}
