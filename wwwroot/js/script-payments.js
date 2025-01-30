// Carregar os gateways de pagamento na inicialização
$.getJSON("/api/payment-gateways", function (data) {
    $.each(data, function (key, value) {
        $("#gatewayDePagamento").append(new Option(value.name, key));
    });
});

function carregaFormaPagamento() {
    var gateway = $("#gatewayDePagamento").val();
    $("#formaPagamento").empty();
    $("#formaPagamentoWrapper").hide();

    if (!gateway) {
        return;
    }
    
    // Buscar métodos de pagamento via API
    $.getJSON("/api/payment-gateways/" + gateway + "/methods", function (data) {

        $("#formaPagamento").append(new Option("Escolha a forma de pagamento", ""));
        $.each(data, function (_, method) {
            $("#formaPagamento").append(new Option(method.name, method.value));
        });

        $("#formaPagamentoWrapper").show();
        $("#labelFormaPagamento").show();
        $("#formaPagamento").show();
    });
}

// Submeter o formulário para gerar cobrança via AJAX
$("#formGerarCobranca").submit(function (e) {
    e.preventDefault();

    var requestData = {
        Id: $("#hfId").val(),
        Tipo: $("#hfTipo").val(),
        FormaPagamento: $("#formaPagamento").val(),
        GatewayDePagamento: $("#gatewayDePagamento").val(),
        Valor: parseFloat($("#valor").val()) || 0
    };

    // Fechar modal antes da requisição
    $("#modalGerarPagamento").modal('hide');

    var form = $(this);
    $.ajax({
        url: "/api/v1/adicionar",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(requestData),
        beforeSend: function () {
            Swal.fire({
                title: 'Processando...',
                text: 'Criando cobrança...',
                icon: 'info',
                allowOutsideClick: false,
                showConfirmButton: false
            });
        },
        success: function (response) {
            console.log("✅ Resposta recebida:", response);
            Swal.fire(response.status, response.message, response.acao).then(() => {
                //window.location.href = "/cobrancas/visualizar/" + response.idCobranca;
            });
        },
        error: function (xhr, status, error) {
            console.error("❌ Erro na requisição:", xhr.responseText);

            if (xhr.status === 403) {
                // Captura erro de permissão e exibe mensagem ao usuário
                var resposta = JSON.parse(xhr.responseText);
                Swal.fire(resposta.status, resposta.message, resposta.acao);
            } else {
                Swal.fire("Erro!", "Erro ao criar cobrança!", "error");
            }
        }
    });
});