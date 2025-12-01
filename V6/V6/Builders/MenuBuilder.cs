using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GJVdc32Tool.Builders
{
    /// <summary>
    /// 菜单构建器
    /// 职责：构建左侧导航菜单
    /// </summary>
    public class MenuBuilder
    {
        #region 常量定义

        private const int BUTTON_HEIGHT = 50;
        private const int BUTTON_MARGIN = 5;

        #endregion

        #region 私有字段

        private readonly Panel _container;
        private readonly List<MenuItemConfig> _menuItems;
        private Color _normalBackColor = Color.FromArgb(45, 45, 48);
        private Color _activeBackColor = Color.FromArgb(62, 62, 66);
        private Color _hoverBackColor = Color.FromArgb(55, 55, 58);
        private Color _foreColor = Color.White;
        private Font _font;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建菜单构建器
        /// </summary>
        /// <param name="container">菜单容器面板</param>
        public MenuBuilder(Panel container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _menuItems = new List<MenuItemConfig>();
            _font = new Font("Segoe UI", 10f, FontStyle.Regular);
        }

        #endregion

        #region 链式配置方法

        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="key">菜单键（用于识别）</param>
        /// <param name="text">显示文本</param>
        /// <param name="icon">图标字符（可选）</param>
        public MenuBuilder AddItem(string key, string text, string icon = null)
        {
            _menuItems.Add(new MenuItemConfig
            {
                Key = key,
                Text = text,
                Icon = icon
            });
            return this;
        }

        /// <summary>
        /// 设置正常状态背景色
        /// </summary>
        public MenuBuilder WithNormalBackColor(Color color)
        {
            _normalBackColor = color;
            return this;
        }

        /// <summary>
        /// 设置激活状态背景色
        /// </summary>
        public MenuBuilder WithActiveBackColor(Color color)
        {
            _activeBackColor = color;
            return this;
        }

        /// <summary>
        /// 设置悬停状态背景色
        /// </summary>
        public MenuBuilder WithHoverBackColor(Color color)
        {
            _hoverBackColor = color;
            return this;
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        public MenuBuilder WithFont(Font font)
        {
            _font = font ?? throw new ArgumentNullException(nameof(font));
            return this;
        }

        #endregion

        #region 构建方法

        /// <summary>
        /// 构建菜单
        /// </summary>
        /// <returns>构建结果</returns>
        public MenuBuildResult Build()
        {
            _container.SuspendLayout();

            try
            {
                _container.Controls.Clear();
                var buttons = new Dictionary<string, Button>();

                int yPos = BUTTON_MARGIN;

                foreach (var item in _menuItems)
                {
                    var button = CreateMenuButton(item, yPos);
                    _container.Controls.Add(button);
                    buttons[item.Key] = button;

                    yPos += BUTTON_HEIGHT + BUTTON_MARGIN;
                }

                // 默认激活第一个
                if (_menuItems.Count > 0)
                {
                    var firstKey = _menuItems[0].Key;
                    if (buttons.ContainsKey(firstKey))
                    {
                        buttons[firstKey].BackColor = _activeBackColor;
                    }
                }

                return new MenuBuildResult
                {
                    Success = true,
                    MenuButtons = buttons,
                    TotalHeight = yPos
                };
            }
            finally
            {
                _container.ResumeLayout(true);
            }
        }

        #endregion

        #region 私有方法

        private Button CreateMenuButton(MenuItemConfig item, int yPos)
        {
            string displayText = string.IsNullOrEmpty(item.Icon)
                ? item.Text
                : $"{item.Icon}  {item.Text}";

            var button = new Button
            {
                Text = displayText,
                Font = _font,
                ForeColor = _foreColor,
                BackColor = _normalBackColor,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Dock = DockStyle.None,
                Size = new Size(_container.Width - 10, BUTTON_HEIGHT),
                Location = new Point(5, yPos),
                Cursor = Cursors.Hand,
                Tag = item.Key
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = _hoverBackColor;
            button.FlatAppearance.MouseDownBackColor = _activeBackColor;

            return button;
        }

        #endregion

        #region 内部类

        private class MenuItemConfig
        {
            public string Key { get; set; }
            public string Text { get; set; }
            public string Icon { get; set; }
        }

        #endregion
    }

    /// <summary>
    /// 菜单构建结果
    /// </summary>
    public class MenuBuildResult
    {
        public bool Success { get; set; }
        public Dictionary<string, Button> MenuButtons { get; set; }
        public int TotalHeight { get; set; }
    }
}
