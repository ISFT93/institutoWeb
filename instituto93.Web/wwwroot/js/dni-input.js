// Formatea en el navegador los inputs dentro de un contenedor .dni-input como 12.345.678.
// Se hace del lado del cliente (y no con MudMask) para no depender de una ida y vuelta
// al servidor por cada tecla, que desordena los dígitos al tipear rápido o desde el celular.
(() => {
    const maxDigits = 8;

    const dniInput = target =>
        target instanceof HTMLInputElement && target.closest('.dni-input') ? target : null;

    const format = digits =>
        [digits.slice(0, 2), digits.slice(2, 5), digits.slice(5, maxDigits)].filter(Boolean).join('.');

    // Al borrar hacia atrás justo después de un punto, borrar el dígito anterior;
    // si no, el punto se volvería a agregar y el cursor quedaría trabado.
    document.addEventListener('beforeinput', event => {
        const input = dniInput(event.target);
        if (!input || event.inputType !== 'deleteContentBackward') return;

        const { selectionStart: start, selectionEnd: end, value } = input;
        if (start !== end || start < 2 || value[start - 1] !== '.') return;

        event.preventDefault();
        input.value = value.slice(0, start - 2) + value.slice(start - 1);
        input.setSelectionRange(start - 2, start - 2);
        input.dispatchEvent(new Event('input', { bubbles: true }));
    }, true);

    // En fase de captura, para que Blazor ya reciba el valor formateado.
    document.addEventListener('input', event => {
        const input = dniInput(event.target);
        if (!input) return;

        const caret = input.selectionStart ?? input.value.length;
        const digitsBeforeCaret = Math.min(input.value.slice(0, caret).replace(/\D/g, '').length, maxDigits);
        const formatted = format(input.value.replace(/\D/g, '').slice(0, maxDigits));

        if (formatted !== input.value) input.value = formatted;

        let position = 0;
        for (let seen = 0; position < formatted.length && seen < digitsBeforeCaret; position++) {
            if (/\d/.test(formatted[position])) seen++;
        }
        input.setSelectionRange(position, position);
    }, true);
})();
