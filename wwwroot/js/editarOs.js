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

$(document).ready(function () {
    $('#formOs').validate({
        rules: {
            'DataInicial': { required: true },
            'DataFinal': { required: true },
            'cliente': { required: true },
            'tecnico': { required: true }
        },
        messages: {
            'DataInicial': { required: 'Campo requerido.' },
            'DataFinal': { required: 'Campo requerido.' },
            'cliente': { required: 'Campo requerido.' },
            'tecnico': { required: 'Campo requerido.' }
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
                url: "/os/editar",
                data: dados,
                dataType: "json",
                success: function (response) {
                    if (response.status === "success") {
                        Swal.fire({
                            icon: response.status,
                            title: "Sucesso",
                            text: "Produto adicionado com sucesso!"
                        });
                    } else {
                        Swal.fire({
                            icon: response.status,
                            title: "Atenção",
                            text: "Produto adicionado com sucesso!"
                        });
                    }
                },
                error: function () {
                    $("#errorUpload").removeClass("d-none").text("Erro ao enviar a imagem.");
                    $("#successUpload").addClass("d-none");
                }
            });
        }
    });
});

$(document).on('click', '#enviarWhatsApp', function () {
    var osId = $(this).data('os-id');

    $.ajax({
        url: `/os/enviar-whatsapp/${osId}`,
        type: 'POST',
        success: function (response) {
            if (response.status === "success") {
                window.open(response.link, '_blank');
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Erro',
                    text: response.message
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: 'Ocorreu um erro ao enviar a mensagem.'
            });
        }
    });
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
        $("#progressBarProdutos").show();
        $.ajax({
            type: "POST",
            url: "/os/adicionar-produto",
            data: dados,
            dataType: "json",
            success: function (response) {
                if (response.status == "success") {
                    let tbody = $(".tblProdutos tbody");
                    tbody.empty();

                    if (response.produtos && Array.isArray(response.produtos)) {
                        response.produtos.forEach(produto => {
                            tbody.append(`
                                    <tr>
                                        <td class="text-left">${produto.produtoNome}</td>
                                        <td class="text-center">${produto.quantidade}</td>
                                        <td class="text-right">${produto.preco}</td>
                                        <td class="text-right">
                                            <a data-id="${produto.id}" role="button" style="margin-right: 1%" class="btn-remover btn-nwe4 text-decoration-none" title="Excluir produto">
                                                <i class="bx bx-trash-alt bx-xs"></i>
                                            </a>
                                        </td>
                                        <td class="text-right">${produto.subTotal}</td>
                                    </tr>
                                `);
                        });
                    }

                    $(".tblProdutos tfoot").html(`
                            <tr>
                                <td colspan="4" class="text-end">
                                    <strong>Total:</strong>
                                </td>
                                <td>
                                    <div class="text-center">
                                        <strong>
                                            ${response.total || "R$ 0,00"}
                                            <input type="hidden" id="total-produto" value="${response.total || "R$ 0,00"}">
                                        </strong>
                                    </div>
                                </td>
                            </tr>
                        `);

                    Swal.fire({
                        icon: "success",
                        title: "Sucesso",
                        text: "Produto adicionado com sucesso!"
                    });

                    $("#produto-quantidade").val("");
                    $("#produto-preco").val("");
                    $("#produto").val("").focus();
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    text: "Não foi possível adicionar o produto. Tente novamente."
                });
            },
            complete: function () {
                $("#progressBarProdutos").hide();
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
                $("#progressBarProdutos").show();

                $.ajax({
                    type: "POST",
                    url: "/os/remover-produto",
                    data: { produtoId: produtoId },
                    dataType: "json",
                    success: function (response) {
                        if (response.status == "success") {
                            let tbody = $(".tblProdutos tbody");
                            tbody.empty();

                            if (response.produtos && Array.isArray(response.produtos)) {
                                response.produtos.forEach(produto => {
                                    tbody.append(`
                                            <tr>
                                                <td class="text-left">${produto.produtoNome}</td>
                                                <td class="text-center">${produto.quantidade}</td>
                                                <td class="text-right">${produto.preco}</td>
                                                <td class="text-right">
                                                    <a data-id="${produto.id}" role="button" style="margin-right: 1%" class="btn-remover btn-nwe4 text-decoration-none" title="Excluir produto">
                                                        <i class="bx bx-trash-alt bx-xs"></i>
                                                    </a>
                                                </td>
                                                <td class="text-right">${produto.subTotal}</td>
                                            </tr>
                                        `);
                                });
                            }

                            $(".tblProdutos tfoot").html(`
                                    <tr>
                                        <td colspan="4" class="text-end">
                                            <strong>Total:</strong>
                                        </td>
                                        <td>
                                            <div class="text-center">
                                                <strong>
                                                    ${response.total || "R$ 0,00"}
                                                    <input type="hidden" id="total-produto" value="${response.total || "R$ 0,00"}">
                                                </strong>
                                            </div>
                                        </td>
                                    </tr>
                                `);

                            Swal.fire({
                                icon: "success",
                                title: "Sucesso",
                                text: "Produto excluido com sucesso!"
                            });

                            $("#produto-quantidade").val("");
                            $("#produto-preco").val("");
                            $("#produto").val("").focus();
                        } else {
                            Swal.fire({
                                icon: "error",
                                title: "Erro",
                                text: response.message
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            icon: "error",
                            title: "Erro",
                            text: "Não foi possível remover o produto. Tente novamente."
                        });
                    },
                    complete: function () {
                        $("#progressBarProdutos").hide();
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
        $("#progressBarServicos").show();
        $.ajax({
            type: "POST",
            url: "/os/adicionar-servico",
            data: dados,
            dataType: "json",
            success: function (response) {
                if (response.status == "success") {
                    let tbody = $(".tblServicos tbody");
                    tbody.empty();

                    if (response.servicos && Array.isArray(response.servicos)) {
                        response.servicos.forEach(servico => {
                            tbody.append(`
                                    <tr>
                                        <td class="text-left">${servico.servicoNome}</td>
                                        <td class="text-center">${servico.quantidade}</td>
                                        <td class="text-right">${servico.preco}</td>
                                        <td class="text-right">
                                            <a data-id="${servico.id}" role="button" style="margin-right: 1%" class="btn-remover-servico btn-nwe4 text-decoration-none" title="Excluir serviço">
                                                <i class="bx bx-trash-alt bx-xs"></i>
                                            </a>
                                        </td>
                                        <td class="text-right">${servico.subTotal}</td>
                                    </tr>
                                `);
                        });
                    }

                    $(".tblServicos tfoot").html(`
                            <tr>
                                <td colspan="4" class="text-end">
                                    <strong>Total:</strong>
                                </td>
                                <td>
                                    <div class="text-center">
                                        <strong>
                                            ${response.total || "R$ 0,00"}
                                            <input type="hidden" id="total-servico" value="${response.total || "R$ 0,00"}">
                                        </strong>
                                    </div>
                                </td>
                            </tr>
                        `);

                    Swal.fire({
                        icon: "success",
                        title: "Sucesso",
                        text: "Serviço adicionado com sucesso!"
                    });

                    $("#servico-quantidade").val("");
                    $("#servico-preco").val("");
                    $("#servico").val("").focus();
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    text: "Não foi possível adicionar o serviço. Tente novamente."
                });
            },
            complete: function () {
                $("#progressBarServicos").hide();
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
                $("#progressBarServicos").show();

                $.ajax({
                    type: "POST",
                    url: "/os/remover-servico",
                    data: { servicoId: servicoId },
                    dataType: "json",
                    success: function (response) {
                        if (response.status == "success") {
                            let tbody = $(".tblServicos tbody");
                            tbody.empty();

                            if (response.servicos && Array.isArray(response.servicos)) {
                                response.servicos.forEach(servico => {
                                    tbody.append(`
                                            <tr>
                                                <td class="text-left">${servico.servicoNome}</td>
                                                <td class="text-center">${servico.quantidade}</td>
                                                <td class="text-right">${servico.preco}</td>
                                                <td class="text-right">
                                                    <a data-id="${servico.id}" role="button" style="margin-right: 1%" class="btn-remover-servico btn-nwe4 text-decoration-none" title="Excluir serviço">
                                                        <i class="bx bx-trash-alt bx-xs"></i>
                                                    </a>
                                                </td>
                                                <td class="text-right">${servico.subTotal}</td>
                                            </tr>
                                        `);
                                });
                            }

                            $(".tblServicos tfoot").html(`
                                    <tr>
                                        <td colspan="4" class="text-end">
                                            <strong>Total:</strong>
                                        </td>
                                        <td>
                                            <div class="text-center">
                                                <strong>
                                                    ${response.total || "R$ 0,00"}
                                                    <input type="hidden" id="total-servico" value="${response.total || "R$ 0,00"}">
                                                </strong>
                                            </div>
                                        </td>
                                    </tr>
                                `);

                            Swal.fire({
                                icon: "success",
                                title: "Sucesso",
                                text: "Serviço excluido com sucesso!"
                            });

                            $("#servico-quantidade").val("");
                            $("#servico-preco").val("");
                            $("#servico").val("").focus();
                        } else {
                            Swal.fire({
                                icon: "error",
                                title: "Erro",
                                text: response.message
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            icon: "error",
                            title: "Erro",
                            text: "Não foi possível remover o serviço. Tente novamente."
                        });
                    },
                    complete: function () {
                        $("#progressBarServicos").hide();
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
                    Swal.fire("Sucesso", response.message, "success");
                    carregarAnexos(osId);
                } else {
                    Swal.fire("Erro", response.message, "error");
                }
            },
            error: function () {
                $("#progressBarAnexos").addClass("d-none");
                Swal.fire("Erro", "Erro ao enviar arquivos.", "error");
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
        $("#progressBarAnotacao").removeClass("d-none");
        $.ajax({
            type: "POST",
            url: "/os/adicionar-anotacao",
            data: dados,
            dataType: "json",
            success: function (response) {
                if (response.status == "success") {
                    let tbody = $(".tblAnotacoes tbody");
                    tbody.empty();

                    if (response.anotacoes && Array.isArray(response.anotacoes)) {
                        response.anotacoes.forEach(anotacao => {
                            tbody.append(`
                                    <tr>
                                        <td class="text-left">${anotacao.dataHora}</td>
                                        <td class="text-left">${anotacao.anotacao}</td>
                                        <td class="text-left">
                                            <a data-id="${anotacao.id}" role="button" style="margin-right: 1%" class="btn-remover-anotacao btn-nwe4 text-decoration-none" title="Excluir anotação">
                                                <i class="bx bx-trash-alt bx-xs"></i>
                                            </a>
                                        </td>
                                    </tr>
                                `);
                        });
                    }

                    Swal.fire({
                        icon: "success",
                        title: "Sucesso",
                        text: "Anotação adicionada com sucesso!",
                        timer: 2000,
                        showConfirmButton: false
                    });

                    // setTimeout(function () {
                    //     $("#modal-adicionar-anotacao").modal("hide");
                    // }, 2000);
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Erro",
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Erro",
                    text: "Não foi possível adicionar a anotação. Tente novamente."
                });
            },
            complete: function () {
                $("#progressBarAnotacao").addClass("d-none");
                $("#modal-adicionar-anotacao").modal("hide");
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
        text: "Essa ação não poderá ser desfeita!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Sim, excluir!",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: `/os/excluir-anotacao/${id}`,
                success: function (response) {
                    if (response.status === "success") {
                        let tbody = $(".tblAnotacoes tbody");
                        tbody.empty();

                        if (response.anotacoes && Array.isArray(response.anotacoes)) {
                            response.anotacoes.forEach(anotacao => {
                                tbody.append(`
                                    <tr>
                                        <td class="text-left">${anotacao.dataHora}</td>
                                        <td class="text-left">${anotacao.anotacao}</td>
                                        <td class="text-left">
                                            <a data-id="${anotacao.id}" role="button" style="margin-right: 1%" class="btn-remover-anotacao btn-nwe4 text-decoration-none" title="Excluir anotação">
                                                <i class="bx bx-trash-alt bx-xs"></i>
                                            </a>
                                        </td>
                                    </tr>
                                `);
                            });
                        }

                        Swal.fire({
                            icon: "success",
                            title: "Excluído",
                            text: "Anotação excluída com sucesso!",
                            timer: 2000,
                            showConfirmButton: false
                        });
                    } else {
                        Swal.fire("Erro!", response.message, "error");
                    }
                },
                error: function () {
                    Swal.fire("Erro!", "Erro ao excluir anotação.", "error");
                }
            });
        }
    });
});

const incrementBtn = document.querySelector('.increment');
const decrementBtn = document.querySelector('.decrement');
const garantiaInput = document.getElementById('garantia');

incrementBtn.addEventListener('click', () => {
    garantiaInput.value = parseInt(garantiaInput.value || 0) + 1;
    updateButtons();
});

decrementBtn.addEventListener('click', () => {
    garantiaInput.value = Math.max(0, parseInt(garantiaInput.value || 0) - 1);
    updateButtons();
});

function updateButtons() {
    decrementBtn.disabled = garantiaInput.value <= 0;
}

updateButtons();

$(document).on("click", ".btn-adicionar-anotacao", function (e) {
    $("#anotacao").val('');
});