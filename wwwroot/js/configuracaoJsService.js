; var ConfiguracaoJsService = (function () {
    var configuracoes = {};

    function carregarConfiguracoes(callback) {
        if (Object.keys(configuracoes).length > 0) {
            if (callback) callback();
            return;
        }

        $.getJSON("/configurar/obter-configuracoes", function (data) {
            configuracoes = data;
            if (callback) callback();
        }).fail(function () {
            console.error("Erro ao carregar configurações.");
        });
    }

    function obterConfiguracao(chave, padrao) {
        return configuracoes[chave] || padrao;
    }

    function obterConfiguracaoBoolean(chave) {
        return configuracoes[chave] === "true" || configuracoes[chave] === true;
    }

    function obterConfiguracaoArray(chave) {
        try {
            return JSON.parse(configuracoes[chave]) || [];
        } catch {
            return [];
        }
    }

    return {
        carregarConfiguracoes: carregarConfiguracoes,
        obterConfiguracao: obterConfiguracao,
        obterConfiguracaoBoolean: obterConfiguracaoBoolean,
        obterConfiguracaoArray: obterConfiguracaoArray
    };
})();