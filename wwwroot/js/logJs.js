$(document).on('click', '.btn-exibir-log', function (e) {
    e.preventDefault();

    var entidade = $(this).data('entidade'); // Nome da entidade
    var entidadeId = $(this).data('id'); // ID da entidade

    // Destrói a tabela existente, se já inicializada
    if ($.fn.DataTable.isDataTable('#tabela-log')) {
        $('#tabela-log').DataTable().destroy();
    }

    $('#log-body').html('<tr><td colspan="6">Carregando...</td></tr>');

    $.ajax({
        url: `/logs/${entidade}/${entidadeId}`,
        method: 'GET',
        success: function (response) {
            if (response.sucesso) {
                var rows = '';
                response.dados.forEach(function (log) {
                    var alteracoes = JSON.parse(log.alteracoes);

                    if (log.tipoOperacao === 'Adição') {
                        alteracoes.forEach(function (alteracao) {
                            rows += `
                                <tr>
                                    <td>${log.usuario}</td>
                                    <td>${new Date(log.dataAlteracao).toLocaleString('pt-BR')}</td>
                                    <td>${alteracao.Coluna}</td>
                                    <td>-</td>
                                    <td>${alteracao.ValorNovo || '-'}</td>
                                    <td>${log.tipoOperacao}</td>
                                </tr>
                            `;
                        });
                    } else {
                        alteracoes.forEach(function (alteracao) {
                            rows += `
                                <tr>
                                    <td>${log.usuario}</td>
                                    <td>${new Date(log.dataAlteracao).toLocaleString('pt-BR')}</td>
                                    <td>${alteracao.Coluna}</td>
                                    <td>${alteracao.ValorAnterior || '-'}</td>
                                    <td>${alteracao.ValorNovo || '-'}</td>
                                    <td>${log.tipoOperacao}</td>
                                </tr>
                            `;
                        });
                    }
                });
                $('#log-body').html(rows);

                // Inicializa o DataTable
                $('#tabela-log').DataTable({
                    language: {
                        url: '/json/datatable_pt-br.json'
                    },
                    paging: true,
                    searching: false,
                    ordering: false,
                    info: true,
                    lengthChange: false,
                    autoWidth: false,
                    pageLength: 15 // Mostra 10 registros por página
                });
            } else {
                $('#log-body').html(`<tr><td colspan="6">${response.mensagem}</td></tr>`);
            }

            // Abrir o modal usando Bootstrap 5
            const modalLog = new bootstrap.Modal(document.getElementById('modal-log'));
            modalLog.show();
        },
        error: function () {
            $('#log-body').html('<tr><td colspan="6">Erro ao carregar os logs.</td></tr>');
        }
    });
});