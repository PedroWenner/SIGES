$('#tabela').DataTable({
    language: {
        url: '/json/datatable_pt-br.json',
        emptyTable: "Nenhum registro encontrado"
    },
    paging: true,           // Habilita paginação
    searching: true,        // Habilita busca
    ordering: true,         // Habilita ordenação
    info: true,             // Habilita informações (ex.: "Mostrando X a Y de Z")
    lengthChange: false,    // Remove o seletor de "quantos itens por página"
    columnDefs: [
        { orderable: false, targets: -1 } // Desativa ordenação na última coluna (Ação)
    ],
    dom: 'lrtip',           // Configuração da estrutura (ajuste conforme necessário)
    autoWidth: false,       // Evita larguras automáticas
    pageLength: 20
});

// Desativa mensagens de erro do DataTables
$.fn.dataTable.ext.errMode = 'none';