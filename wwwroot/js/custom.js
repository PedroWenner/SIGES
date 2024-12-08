function BindControlEvents() {
    $("table").addClass("table table-borderless table-hover table-custom");
    $("input[type='text']").addClass("form-control form-control-sm");
    $("input[type='file']").addClass("form-control form-control-sm");
    $("input[type='password']").addClass("form-control form-control-sm");
    $("input[type='date']").addClass("form-control form-control-sm");

    $('.datetime').datepicker();

    var elementsWithTitle = document.querySelectorAll('[title]');
    elementsWithTitle.forEach(function (element) {
        element.classList.add('tip-bottom');
    });

    $("select").each(function () {
        var currentId = $(this).attr('multiple');
        if ($(this).attr('id') != undefined) {
            if (typeof currentId === 'undefined' && $(this).attr('id').indexOf('_list') == -1 && $(this).attr('id').indexOf('_lst') == -1 && $(this).attr('id').indexOf('_chklist') == -1) {
                $(this).attr('data-plugin-selectTwo', 'data-plugin-selectTwo');
                //$(this).addClass("form-control populate input-sm mb-md");
            }
        }
    });

    (function ($) {
        'use strict';

        if ($.isFunction($.fn['select2'])) {
            $(function () {
                $('[data-plugin-selectTwo]').each(function () {
                    var $this = $(this),
                        opts = {};

                    var pluginOptions = $this.data('plugin-options');
                    if (pluginOptions)
                        opts = pluginOptions;

                    $this.themePluginSelect2(opts);
                });
            });
        }
    }).apply(this, [jQuery]);
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