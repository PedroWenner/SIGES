function BindControlEvents() {
    $("table").addClass("table table-borderless table-hover table-custom");
    $("input[type='text']").addClass("form-control form-control-sm");
    $("input[type='file']").addClass("form-control form-control-sm");
    $("input[type='password']").addClass("form-control form-control-sm");
    $("input[type='date']").addClass("form-control form-control-sm");
    $("select").addClass("form-select form-select-sm");

    $('.datetime').datepicker();

    var elementsWithTitle = document.querySelectorAll('[title]');
    elementsWithTitle.forEach(function (element) {
        element.classList.add('tip-bottom');
    });

    // Aqui será percorrido todos os inputs com required para atribuir o *
    const requiredFields = document.querySelectorAll("[required]");
    requiredFields.forEach((field) => {
        const label = field.closest("form").querySelector(`label[for='${field.id}']`);
        if (label && !label.innerHTML.includes("*")) {
            label.innerHTML += ' <span class="text-danger">*</span>';
        }
    });
    //

}

$(document).ready(function () {
    $.datepicker.regional['pt-BR'] = {
        closeText: 'Fechar',
        prevText: '&#x3C;Anterior',
        nextText: 'Próximo&#x3E;',
        currentText: 'Hoje',
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
            'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
            'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
        dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);

    BindControlEvents();
});