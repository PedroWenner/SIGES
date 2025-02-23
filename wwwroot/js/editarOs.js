$(document).ready(function () {
    $(".money").mask("#.##0,00", { reverse: true });
});

$('#descricao-produto').trumbowyg({
    lang: 'pt_br',
    autogrow: true,
    tagsToRemove: ['script', 'link'],
    tagsToKeep: ['i', 'script[src]'],
    btns: [
        ['fontfamily'],
        ['fontsize'],
        ['undo', 'redo'],
        ['bold', 'italic'],
        ['formatting'],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
        ['unorderedList', 'orderedList'],
        ['horizontalRule'],
    ],
    plugins: {
        fontsize: {
            sizeList: [
                '10px', '12px', '14px', '16px', '18px', '20px', '24px', '28px', '32px', '36px'
            ]
        }
    }
});

$('#defeito').trumbowyg({
    lang: 'pt_br',
    autogrow: true,
    tagsToRemove: ['script', 'link'],
    tagsToKeep: ['i', 'script[src]'],
    btns: [
        ['fontfamily'],
        ['fontsize'],
        ['undo', 'redo'],
        ['bold', 'italic'],
        ['formatting'],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
        ['unorderedList', 'orderedList'],
        ['horizontalRule'],
    ],
    plugins: {
        fontsize: {
            sizeList: [
                '10px', '12px', '14px', '16px', '18px', '20px', '24px', '28px', '32px', '36px'
            ]
        }
    }
});

$('#observacoes').trumbowyg({
    lang: 'pt_br',
    autogrow: true,
    tagsToRemove: ['script', 'link'],
    tagsToKeep: ['i', 'script[src]'],
    btns: [
        ['fontfamily'],
        ['fontsize'],
        ['undo', 'redo'],
        ['bold', 'italic'],
        ['formatting'],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
        ['unorderedList', 'orderedList'],
        ['horizontalRule'],
    ],
    plugins: {
        fontsize: {
            sizeList: [
                '10px', '12px', '14px', '16px', '18px', '20px', '24px', '28px', '32px', '36px'
            ]
        }
    }
});

$('#laudo-tecnico').trumbowyg({
    lang: 'pt_br',
    autogrow: true,
    tagsToRemove: ['script', 'link'],
    tagsToKeep: ['i', 'script[src]'],
    btns: [
        ['fontfamily'],
        ['fontsize'],
        ['undo', 'redo'],
        ['bold', 'italic'],
        ['formatting'],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
        ['unorderedList', 'orderedList'],
        ['horizontalRule'],
    ],
    plugins: {
        fontsize: {
            sizeList: [
                '10px', '12px', '14px', '16px', '18px', '20px', '24px', '28px', '32px', '36px'
            ]
        }
    }
});

$(function () {
    $("#cliente").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/clientes/ListarClientes",
                type: "GET",
                data: { term: request.term },
                success: function (data) {
                    if (data.length === 0) {
                        response([{
                            nome: "Adicionar cliente",
                            value: "add",
                            adicionar: true
                        }]);
                    } else {
                        response(data);
                    }
                }
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            if (!ui.item.adicionar) {
                $("#cliente").val(ui.item.nome);
            }

            return false;
        },
        select: function (event, ui) {
            if (ui.item.adicionar) {
                window.location.href = "/clientes/adicionar";
            } else {
                $("#cliente").val(ui.item.nome);
                $("#cliente-id").val(ui.item.value);
            }

            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        if (item.adicionar) {
            return $("<li>")
                .append(
                    `<div class="ui-menu-item-wrapper">
                            <span class="autocomplete-add">➕ ${item.nome}</span>
                        </div>`
                )
                .appendTo(ul);
        } else {
            return $("<li>")
                .append(
                    `<div class="ui-menu-item-wrapper">
                            <span class="autocomplete-title">${item.nome}</span>
                            <span class="autocomplete-details">Celular: ${item.celular} | Documento: ${item.documento}</span>
                        </div>`
                )
                .appendTo(ul);
        }
    };
});

$(function () {
    $("#vendedor").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/usuarios/listarUsuarios",
                type: "GET",
                data: { term: request.term },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            $("#vendedor").val(ui.item.nome);
            return false;
        },
        select: function (event, ui) {
            $("#vendedor").val(ui.item.nome);
            $("#vendedor-id").val(ui.item.value);
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append(
                `<div class="ui-menu-item-wrapper">
                        <span class="autocomplete-title">${item.nome}</span>
                        <span class="autocomplete-details">Celular: ${item.celular}</span>
                    </div>`
            )
            .appendTo(ul);
    };
});

$(function () {
    $("#garantias").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/garantias/listarGarantias",
                type: "GET",
                data: { term: request.term },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            $("#garantias").val(ui.item.nome);
            return false;
        },
        select: function (event, ui) {
            $("#garantias").val(ui.item.nome);
            $("#garantias-id").val(ui.item.value);
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append(
                `<div class="ui-menu-item-wrapper">
                        <span class="autocomplete-title">${item.nome}</span>
                    </div>`
            )
            .appendTo(ul);
    };
});

$(function () {
    $("#servico").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/servicos/listar-servicos",
                type: "GET",
                data: { term: request.term },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            $("#servico").val(ui.item.nome + " | Valor: R$ " + ui.item.valor);
            return false;
        },
        select: function (event, ui) {
            $("#servico").val(ui.item.nome + " | Valor: R$ " + ui.item.valor);
            $("#servico-id").val(ui.item.value);
            $("#servico-preco").val(ui.item.valor);
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append(
                `<div class="ui-menu-item-wrapper">
                        <span class="autocomplete-title">${item.nome}</span>
                        <span class="autocomplete-details">Valor: ${item.valor}</span>
                    </div>`
            )
            .appendTo(ul);
    };
});

$(function () {
    $("#produto").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/produtos/listarProdutos",
                type: "GET",
                data: { term: request.term },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 2,
        focus: function (event, ui) {
            $("#produto").val(ui.item.nome + " | Preço: R$ " + ui.item.preco + " | Estoque: " + ui.item.estoque);
            return false;
        },
        select: function (event, ui) {
            $("#produto").val(ui.item.nome + " | Preço: R$ " + ui.item.preco + " | Estoque: " + ui.item.estoque);
            $("#produto-id").val(ui.item.value);

            let precoFormatado = parseFloat(ui.item.preco).toLocaleString('pt-BR', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
            $("#produto-preco").val(precoFormatado);

            $("#produto-quantidade").data("estoque", ui.item.estoque);
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append(
                `<div class="ui-menu-item-wrapper">
                        <span class="autocomplete-title">${item.nome}</span>
                        <span class="autocomplete-details">Preço: R$ ${item.preco.toLocaleString('pt-BR', {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                })} | Estoque: ${item.estoque}</span>
                    </div>`
            )
            .appendTo(ul);
    };
});

$(document).on('click', '#enviarWhatsApp', function () {
    var id = $(this).data('id');
    var tipo = $(this).data('tipo');

    if (tipo === 'pix') {
        var mensagem = $("#pixCodeText").val();
    } else {
        var mensagem = '';
    }

    $.ajax({
        url: `/api/mensagem/enviar-whatsapp/${id}`,
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify({ mensagem: mensagem }),
        success: function (response) {
            if (response.status === "success") {
                window.open(response.link, '_blank');
            } else {
                Swal.fire({
                    icon: response.status,
                    title: response.title,
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: 'Ocorreu um erro ao enviar a mensagem.',
                timer: 2000,
                showConfirmButton: false
            });
        }
    });
});

$("#formProdutos").validate({
    rules: {
        Preco: {
            required: true
        },
        Quantidade: {
            required: true
        }
    },
    messages: {
        Preco: {
            required: 'Inserir o preço'
        },
        Quantidade: {
            required: 'Insira a quantidade'
        }
    },
    errorPlacement: function (error, element) {
        element.attr("placeholder", error.text());
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass("is-invalid").removeClass("is-valid");
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass("is-invalid").addClass("is-valid");
        $(element).attr("placeholder", "");
    },
    submitHandler: function (form) {
        var dados = $(form).serialize();
        $("#modal-spinner").modal('show');

        $.ajax({
            type: "POST",
            url: "/os/adicionar-produto",
            data: dados,
            dataType: "json",
            success: function (response) {
                if (response.status == "success") {
                    setTimeout(function () {
                        atualizarTabelaProdutos(response.os_id);
                        $("#modal-spinner").modal('hide');

                        Swal.fire({
                            icon: "success",
                            title: "Sucesso",
                            text: "Produto adicionado com sucesso!",
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }, 1500);
                } else {
                    $("#modal-spinner").modal('hide');
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: response.message,
                        timer: 2000,
                        showConfirmButton: false
                    });
                }

                $("#produto-quantidade").val("");
                $("#produto-preco").val("");
                $("#produto").val("").focus();
            },
            error: function () {
                $("#modal-spinner").modal('hide');
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    text: "Não foi possível adicionar o produto. Tente novamente.",
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        });

        return false;
    }
});

$(document).ready(function () {
    $(document).on("click", ".btn-remover", function (event) {
        event.preventDefault();

        var produtoId = $(this).data("id");

        Swal.fire({
            title: "Tem certeza?",
            text: "Você deseja remover este item?",
            icon: "warning",
            showCancelButton: true,
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sim, remover!"
        }).then((result) => {
            if (result.isConfirmed) {
                $("#modal-spinner").modal('show');

                $.ajax({
                    type: "POST",
                    url: "/os/remover-produto",
                    data: { produtoId: produtoId },
                    dataType: "json",
                    success: function (response) {
                        if (response.status == "success") {
                            setTimeout(function () {
                                atualizarTabelaProdutos(response.os_id);
                                $("#modal-spinner").modal('hide');

                                Swal.fire({
                                    icon: "success",
                                    title: "Sucesso",
                                    text: "Produto excluido com sucesso!",
                                    timer: 2000,
                                    showConfirmButton: false
                                });
                            }, 1500);
                        } else {
                            $("#modal-spinner").modal('hide');
                            Swal.fire({
                                icon: "error",
                                title: "Erro",
                                text: response.message,
                                timer: 2000,
                                showConfirmButton: false
                            });
                        }

                        $("#produto-quantidade").val("");
                        $("#produto-preco").val("");
                        $("#produto").val("").focus();
                    },
                    error: function () {
                        $("#modal-spinner").modal('hide');
                        Swal.fire({
                            icon: "error",
                            title: "Erro",
                            text: "Não foi possível remover o produto. Tente novamente.",
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }
                });
            }
        });
    });
});

$("#formServicos").validate({
    rules: {
        Preco: {
            required: true
        },
        Quantidade: {
            required: true
        }
    },
    messages: {
        Preco: {
            required: 'Inserir o preço'
        },
        Quantidade: {
            required: 'Insira a quantidade'
        }
    },
    errorPlacement: function (error, element) {
        element.attr("placeholder", error.text());
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass("is-invalid").removeClass("is-valid");
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass("is-invalid").addClass("is-valid");
        $(element).attr("placeholder", "");
    },
    submitHandler: function (form) {
        var dados = $(form).serialize();
        $("#modal-spinner").modal('show');
        $.ajax({
            type: "POST",
            url: "/os/adicionar-servico",
            data: dados,
            dataType: "json",
            success: function (response) {
                if (response.status == "success") {
                    setTimeout(function () {
                        atualizarTabelaServicos(response.os_id);
                        $("#modal-spinner").modal('hide');
                        Swal.fire({
                            icon: "success",
                            title: "Sucesso",
                            text: "Serviço adicionado com sucesso!",
                            timer: 2000,
                            showConfirmButton: false
                        });

                        $("#servico-quantidade").val("");
                        $("#servico-preco").val("");
                        $("#servico").val("").focus();
                    }, 1500);
                } else {
                    $("#modal-spinner").modal('hide');
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: response.message,
                        timer: 2000,
                        showConfirmButton: false
                    });
                }
            },
            error: function () {
                $("#modal-spinner").modal('hide');
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    text: "Não foi possível adicionar o serviço. Tente novamente.",
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        });

        return false;
    }
});

$(document).ready(function () {
    $(document).on("click", ".btn-remover-servico", function (event) {
        event.preventDefault();

        var servicoId = $(this).data("id");

        Swal.fire({
            title: "Tem certeza?",
            text: "Você deseja remover este serviço?",
            icon: "warning",
            showCancelButton: true,
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sim, remover!"
        }).then((result) => {
            if (result.isConfirmed) {
                $("#modal-spinner").modal('show');

                $.ajax({
                    type: "POST",
                    url: "/os/remover-servico",
                    data: { servicoId: servicoId },
                    dataType: "json",
                    success: function (response) {
                        if (response.status == "success") {
                            setTimeout(function () {
                                atualizarTabelaServicos(response.os_id);
                                $("#modal-spinner").modal('hide');

                                Swal.fire({
                                    icon: "success",
                                    title: "Sucesso",
                                    text: "Serviço excluido com sucesso!",
                                    timer: 2000,
                                    showConfirmButton: false
                                });

                                $("#servico-quantidade").val("");
                                $("#servico-preco").val("");
                                $("#servico").val("").focus();
                            }, 1500);
                        } else {
                            $("#modal-spinner").modal('hide');

                            Swal.fire({
                                icon: "error",
                                title: "Erro",
                                text: response.message,
                                timer: 2000,
                                showConfirmButton: false
                            });
                        }
                    },
                    error: function () {
                        $("#modal-spinner").modal('hide');

                        Swal.fire({
                            icon: "error",
                            title: "Erro",
                            text: "Não foi possível remover o serviço. Tente novamente.",
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }
                });
            }
        });
    });
});

$(document).ready(function () {
    var osId = $("input[name='OsId']").val();
    carregarAnexos(osId);

    $("#formAnexos").submit(function (e) {
        e.preventDefault();

        var formData = new FormData(this);
        formData.append("osId", osId);

        $("#progressBarAnexos").removeClass("d-none");

        $.ajax({
            type: "POST",
            url: "/os/anexar",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                $("#progressBarAnexos").addClass("d-none");

                if (response.status == "success") {
                    Swal.fire({
                        icon: "success",
                        title: "Sucesso!",
                        text: response.message,
                        timer: 2000,
                        showConfirmButton: false
                    });
                    carregarAnexos(osId);
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Erro!",
                        text: response.message,
                        timer: 2000,
                        showConfirmButton: false
                    });
                }
            },
            error: function () {
                $("#progressBarAnexos").addClass("d-none");
                Swal.fire({
                    icon: "error",
                    title: "Erro!",
                    text: "Erro ao enviar arquivos.",
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        });
    });
});

// Função para buscar e carregar anexos existentes
function carregarAnexos(osId) {
    $.get(`/os/listar-anexos/${osId}`, function (response) {
        if (response.status === "success") {
            atualizarListaAnexos(response.arquivos);
        }
    });
}

// Função para atualizar dinamicamente os anexos na tela
function atualizarListaAnexos(arquivos) {
    var divAnexos = $("#divAnexos");
    divAnexos.empty();

    if (arquivos.length === 0) {
        divAnexos.html('<div class="col-12 text-center"><p class="text-muted">Nenhum anexo disponível.</p></div>');
        return;
    }

    arquivos.forEach(function (anexo) {
        var fileName = anexo.split('/').pop();
        var isImage = fileName.match(/\.(jpg|jpeg|png|gif)$/i);
        var thumbnail = isImage ? anexo : "/img/documento-de-texto.png";

        var html = `
                <div class="col-6 col-sm-4 col-md-3 col-lg-2 mb-3">
                    <div class="card card-custom text-center shadow-sm">
                        <a href="${anexo}" target="_blank" class="d-block p-2">
                            <img src="${thumbnail}" alt="Anexo" class="img-fluid rounded" style="max-height: 120px;">
                        </a>
                        <div class="card-body card-body-custom p-2">
                            <small class="d-block text-truncate text-truncate-custom" style="max-width: 100px;">${anexo.split('/').pop()}</small>
                            <div class="d-flex justify-content-center gap-2 mt-2">
                                <a href="/os/download/${fileName}" class="btn btn-sm btn-secondary" title="Baixar">
                                    <i class="fas fa-download"></i>
                                </a>
                                <a role="button" href="/os/excluir-anexo/${fileName}" class="btn btn-sm btn-danger btn-remover-anexo" data-id="${fileName}" title="Remover">
                                    <i class="bx bx-trash"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>`;

        divAnexos.append(html);
    });
}

$("#form-adicionar-anotacao").validate({
    rules: {
        Anotacao: {
            required: true
        }
    },
    messages: {
        Anotacao: {
            required: 'Inserir a anotação'
        }
    },
    errorPlacement: function (error, element) {
        element.attr("placeholder", error.text());
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass("is-invalid").removeClass("is-valid");
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass("is-invalid").addClass("is-valid");
        $(element).attr("placeholder", "");
    },
    submitHandler: function (form) {
        var dados = $(form).serialize();
        $("#modal-adicionar-anotacao").modal("hide");
        $("#modal-spinner").modal('show');
        $.ajax({
            type: "POST",
            url: "/os/adicionar-anotacao",
            data: dados,
            dataType: "json",
            success: function (response) {
                if (response.status == "success") {
                    setTimeout(function () {
                        atualizarTabelaAnotacoes(response.os_id);
                        $("#modal-spinner").modal('hide');

                        Swal.fire({
                            icon: "success",
                            title: "Sucesso",
                            text: "Anotação adicionada com sucesso!",
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }, 1500);
                } else {
                    $("#modal-spinner").modal('hide');
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: response.message,
                        timer: 2000,
                        showConfirmButton: false
                    });
                }
            },
            error: function () {
                $("#modal-spinner").modal('hide');
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    text: "Não foi possível adicionar a anotação. Tente novamente.",
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        });

        return false;
    }
});

$(document).on("click", ".btn-remover-anotacao", function (e) {
    e.preventDefault();
    var id = $(this).data("id");

    Swal.fire({
        title: "Tem certeza?",
        text: "Você deseja remover esta anotação?",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Cancelar",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sim, remover!"
    }).then((result) => {
        if (result.isConfirmed) {
            $("#modal-spinner").modal('show');
            $.ajax({
                type: "POST",
                url: `/os/excluir-anotacao/${id}`,
                success: function (response) {
                    if (response.status === "success") {
                        setTimeout(function () {
                            atualizarTabelaAnotacoes(response.os_id);
                            $("#modal-spinner").modal('hide');

                            Swal.fire({
                                icon: "success",
                                title: "Excluído",
                                text: "Anotação excluída com sucesso!",
                                timer: 2000,
                                showConfirmButton: false
                            });
                        }, 1500);
                    } else {
                        $("#modal-spinner").modal('hide');
                        Swal.fire({
                            icon: "error",
                            title: "Erro!",
                            text: response.message,
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }
                },
                error: function () {
                    $("#modal-spinner").modal('hide');
                    Swal.fire({
                        icon: "error",
                        title: "Erro!",
                        text: "Erro ao excluir anotação.",
                        timer: 2000,
                        showConfirmButton: false
                    });
                }
            });
        }
    });
});

function increment(btnDecrement, id) {
    const input = document.getElementById(id);
    const decrement = document.getElementById(btnDecrement);
    input.value = parseInt(input.value || 0) + 1;
    updateButtons(decrement, input);
}

function decrement(btnDecrement, id) {
    const input = document.getElementById(id);
    const decrement = document.getElementById(btnDecrement);
    input.value = parseInt(input.value || 0) - 1;
    updateButtons(decrement, input);
}

function updateButtons(btnDecrement, id) {
    btnDecrement.disabled = id.value <= 0;
}

$(document).on("click", ".btn-adicionar-anotacao", function (e) {
    $("#anotacao").val('');
});

function calcularDesconto() {
    let valor = parseFloat($('#valor-total-os').val().replace('.', '').replace(',', '.')) || 0;
    var tipoDesconto = $('#tipo-desconto').val();
    let desconto = parseFloat($('#desconto').val()) || 0;

    if (valor === 0)
        return false;

    if (tipoDesconto == 'porcento') {
        if (desconto > 100) {
            Swal.fire("Erro!", "Não é possível aplicar desconto maior que 100%.", "error");
            $("#desconto").val('');
            return false;
        }
        
        valor -= (valor * desconto / 100);
        $("#valor-desconto").val(valor.toLocaleString("pt-BR", { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
    } else {
        if (desconto > valor) {
            Swal.fire("Erro!", "Não é possível aplicar um desconto maior que o valor total da venda.", "error");
            $("#desconto").val('');
            return false;
        }
        
        valor -= desconto;
        $("#valor-desconto").val(valor.toLocaleString("pt-BR", { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
    }
}

$("#form-desconto").validate({
    rules: {
        Desconto: { required: true },
        ValorDesconto: { required: true }
    },
    messages: {
        Desconto: { required: 'Inserir o desconto' },
        ValorDesconto: { required: 'Inserir o total com o desconto' }
    },
    errorPlacement: function (error, element) {
        element.attr("placeholder", error.text());
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass("is-invalid").removeClass("is-valid");
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass("is-invalid").addClass("is-valid");
        $(element).attr("placeholder", "");
    },
    submitHandler: function (form) {
        var dados = $(form).serialize();
        $.ajax({
            type: "POST",
            url: "/os/aplicar-desconto",
            data: dados,
            dataType: "json",
            beforeSend: function () {
                Swal.fire({
                    title: 'Processando',
                    text: 'Aplicando desconto...',
                    icon: 'info',
                    showCloseButton: false,
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    allowEscapeKey: false
                });
            },
            success: function (response) {
                if (response.status == "success") {
                    setTimeout(function () {
                        Swal.fire({
                            icon: "success",
                            title: "Sucesso",
                            text: "Desconto aplicado com sucesso!",
                            timer: 2000,
                            showConfirmButton: false
                        });
                    }, 1500);
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: response.message,
                        timer: 2000,
                        showConfirmButton: false
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    text: "Não foi possível adicionar o desconto. Tente novamente.",
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        });

        return false;
    }
});

$(document).on('click', '#enviar-email', function () {
    var id = $(this).data('id');
    $.ajax({
        url: `/api/mensagem/enviar-email/${id}`,
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify({ assunto: "Ordem de Serviço", tipo: "os", nomeview: "Email/Os/OsEmail", automaticoMail: "S" }),
        success: function (response) {
            if (response.status === "success") {
                Swal.fire({
                    icon: response.status,
                    title: response.title,
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
            } else {
                Swal.fire({
                    icon: response.status,
                    title: response.title,
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: 'Ocorreu um erro ao enviar a mensagem.',
                timer: 2000,
                showConfirmButton: false
            });
        }
    });
});

function atualizarTabelaProdutos(id) {
    $.ajax({
        type: "GET",
        url: "/os/obter-produtos/" + id,
        dataType: "json",
        success: function (response) {
            let tbody = $(".tblProdutos tbody");
            tbody.empty();

            if (response.produtos && Array.isArray(response.produtos)) {
                response.produtos.forEach(p => {
                    tbody.append(`
                        <tr>
                            <td style="width: 60%" class="text-end">${p.nome}</td>
                            <td style="width: 10%" class="text-center">${p.quantidade}</td>
                            <td style="width: 10%" class="text-center">R$ ${p.preco}</td>
                            <td style="width: 10%" class="text-center">R$ ${p.subtotal}</td>
                            <td style="width: 10%" class="text-center">
                                <a data-id="${p.id}" role="button" style="margin-right: 1%" class="btn-remover btn-nwe4 text-decoration-none" title="Excluir produto">
                                    <i class="bx bx-trash-alt bx-xs"></i>
                                </a>
                            </td>
                            
                        </tr>
                    `);
                });
            } else {
                tbody.append(`
                    <tr>
                        <td colspan="5" class="text-center">Nenhum produto adicionado.</td>
                    </tr>
                `);
            }

            // Atualiza os totais na tabela
            $(".tblProdutos tfoot").html(`
                <tr>
                    <td colspan="3" class="text-end"><strong>Total:</strong></td>
                    <td colspan="2" class="text-start">R$ ${response.total || "0,00"}</td>
                </tr>
            `);

            $('#valor-total-os').val(response.totalos);
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Erro",
                text: "Não foi possível atualizar a tabela. Tente novamente."
            });
        }
    });
}
function atualizarTabelaServicos(id) {
    $.ajax({
        type: "GET",
        url: "/os/obter-servicos/" + id,
        dataType: "json",
        success: function (response) {
            let tbody = $(".tblServicos tbody");
            tbody.empty();

            if (response.servicos && Array.isArray(response.servicos)) {
                response.servicos.forEach(s => {
                    tbody.append(`
                        <tr>
                            <td style="width: 60%" class="text-end">${s.nome}</td>
                            <td style="width: 10%" class="text-center">${s.quantidade}</td>
                            <td style="width: 10%" class="text-center">R$ ${s.preco}</td>
                            <td style="width: 10%" class="text-center">R$ ${s.subtotal}</td>
                            <td style="width: 10%" class="text-center">
                                <a data-id="${s.id}" role="button" style="margin-right: 1%" class="btn-remover-servico btn-nwe4 text-decoration-none" title="Excluir serviço">
                                    <i class="bx bx-trash-alt bx-xs"></i>
                                </a>
                            </td>
                            
                        </tr>
                    `);
                });
            } else {
                tbody.append(`
                    <tr>
                        <td colspan="5" class="text-center">Nenhum serviço adicionado.</td>
                    </tr>
                `);
            }

            // Atualiza os totais na tabela
            $(".tblServicos tfoot").html(`
                <tr>
                    <td colspan="3" class="text-end"><strong>Total:</strong></td>
                    <td colspan="2" class="text-start">R$ ${response.total || "0,00"}</td>
                </tr>
            `);

            $('#valor-total-os').val(response.totalos);
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Erro",
                text: "Não foi possível atualizar a tabela. Tente novamente."
            });
        }
    });
}
function atualizarTabelaAnotacoes(id) {
    $.ajax({
        type: "GET",
        url: "/os/obter-anotacoes/" + id,
        dataType: "json",
        success: function (response) {
            let tbody = $(".tblAnotacoes tbody");
            tbody.empty();

            if (response.anotacoes && Array.isArray(response.anotacoes)) {
                response.anotacoes.forEach(a => {
                    tbody.append(`
                        <tr>
                            <td style="width: 20%" class="text-end">${a.datahora}</td>
                            <td style="width: 70%" class="text-left">${a.anotacao}</td>
                            <td style="width: 10%" class="text-center">
                                <a data-id="${a.id}" role="button" style="margin-right: 1%" class="btn-remover-anotacao btn-nwe4 text-decoration-none" title="Excluir anotação">
                                    <i class="bx bx-trash-alt bx-xs"></i>
                                </a>
                            </td>
                            
                        </tr>
                    `);
                });
            } else {
                tbody.append(`
                    <tr>
                        <td colspan="3" class="text-center">Nenhuma anotação adicionada.</td>
                    </tr>
                `);
            }
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Erro",
                text: "Não foi possível atualizar a tabela. Tente novamente."
            });
        }
    });
}