// Envía el formulario de cierre de login como POST nativo (navegación completa), para que
// el servidor pueda emitir la cookie de sesión. Blazor no intercepta form.submit().
window.instituto93Auth = {
  completeLogin(formId, ticket) {
    const form = document.getElementById(formId);
    form.elements.ticket.value = ticket;
    form.submit();
  }
};
