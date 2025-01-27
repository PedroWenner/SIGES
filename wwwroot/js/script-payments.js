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

    console.log("📌 Gateway selecionado:", gateway);

    if (!gateway) {
        return;
    }
    
    // Buscar métodos de pagamento via API
    $.getJSON("/api/payment-gateways/" + gateway + "/methods", function (data) {
        console.log("✅ Métodos de pagamento recebidos:", data);

        $("#formaPagamento").append(new Option("Escolha a forma de pagamento", ""));
        $.each(data, function (_, method) {
            $("#formaPagamento").append(new Option(method.name, method.value));
        });

        console.log("🟢 Exibindo #formaPagamentoWrapper");
        $("#formaPagamentoWrapper").show();
        $("#labelFormaPagamento").show();
        $("#formaPagamento").show();
    });
}

// Submeter o formulário para gerar cobrança via AJAX
$("#formGerarCobranca").submit(function (e) {
    e.preventDefault();

    var requestData = {
        Id: $("#id").val(),
        Tipo: "venda", // Defina isso corretamente
        FormaPagamento: $("#formaPagamento").val(),
        GatewayDePagamento: $("#gatewayDePagamento").val(),
        Valor: parseFloat($("#valor").val()) || 0
    };

    console.log("📌 Enviando requisição:", requestData);

    // Se o campo de forma de pagamento estiver oculto, remova o atributo "required"
    if ($("#formaPagamento").is(":hidden")) {
        $("#formaPagamento").removeAttr("required");
    }

    // Fechar modal antes da requisição
    $("#modalGerarPagamento").modal('hide');

    var form = $(this);
    $.ajax({
        url: "/api/cobrancas/adicionar", // Certifique-se que essa URL está correta
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(requestData),
        beforeSend: function () {
            console.log("⏳ Enviando solicitação...");
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
            Swal.fire("Sucesso!", "Cobrança criada com sucesso!", "success").then(() => {
                window.location.href = "/cobrancas/visualizar/" + response.idCobranca;
            });
        },
        error: function (xhr, status, error) {
            console.error("❌ Erro na requisição:", xhr.responseText);
            Swal.fire("Erro!", "Erro ao criar cobrança!", "error");
        }
    });
});