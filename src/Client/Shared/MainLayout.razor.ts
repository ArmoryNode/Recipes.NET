// if the user's preference is not stored in localstorage, then we'll initialize it to the browser's prefered setting
export function initializeDarkModePreference(key: string): void {
    if (localStorage.getItem(key) === null) {
        const darkModeQuery = window.matchMedia("(prefers-color-scheme: dark)");
        const enableDarkMode = darkModeQuery.matches.toString();
        localStorage.setItem(key, enableDarkMode.toString());
    }
}