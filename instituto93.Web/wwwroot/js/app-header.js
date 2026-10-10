// Envía el form de logout del header (AppHeader.razor).
// La cookie de sesión HttpOnly solo se puede borrar desde una request HTTP,
// por eso se hace un POST a /account/logout en vez de cerrar desde el circuito.
window.appHeader = {
    submitLogout: function () {
        var form = document.getElementById('app-header-logout-form');
        if (!form) return;

        if (typeof form.requestSubmit === 'function') {
            form.requestSubmit();
        } else {
            form.submit();
        }
    }
};
