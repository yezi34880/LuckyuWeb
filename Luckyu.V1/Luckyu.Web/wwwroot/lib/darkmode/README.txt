<script src="https://cdn.jsdelivr.net/npm/@cxy227/dark-mode-js@0.1.2/dist/index.js"></script>
        <script>
            function init() {   // 黑暗模式测试
                const darkmode = new darkModeJs.darkMode()

                darkmode.showWidget();
                //darkmode.setDarkTheme();
                //darkmode.setLightTheme();
            }

            window.addEventListener('load', init);
        </script>