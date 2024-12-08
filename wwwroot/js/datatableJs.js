$('#tabela').DataTable({
    language: {
        url: '/json/datatable_pt-br.json',
    },
    //ordering: true, ativa/desativa a ordenação
    paging: false, // ativa/desativa a função de paginação
    responsive: true,
    searching: true, // ativa/desativa a função de pesquisa
    info: false, // ativa/desativa a quantidade de registros por página
    columnDefs: [
        { orderable: false, targets: -1 } // Desativa ordenação na última coluna (Ação)
    ]
});