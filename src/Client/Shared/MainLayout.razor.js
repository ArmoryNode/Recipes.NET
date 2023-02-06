export function initializeDarkModePreference(key) {
    if (localStorage.getItem(key) === null) {
        const darkModeQuery = window.matchMedia("(prefers-color-scheme: dark)");
        const enableDarkMode = darkModeQuery.matches.toString();
        localStorage.setItem(key, enableDarkMode.toString());
    }
}
//# sourceMappingURL=MainLayout.razor.js.map