ConfiguracaoJsService.carregarConfiguracoes(function () {
    var pageLength = parseInt(ConfiguracaoJsService.obterConfiguracao("per_page", 10));

    $('#tabela').DataTable({
        language: {
            url: '/json/datatable_pt-br.json',
            emptyTable: "Nenhum registro encontrado"
        },
        paging: true,
        searching: true,
        ordering: false,
        info: true,
        lengthChange: false,
        autoWidth: false,
        pageLength: pageLength
    });
});

// Desativa mensagens de erro do DataTables
$.fn.dataTable.ext.errMode = 'none';