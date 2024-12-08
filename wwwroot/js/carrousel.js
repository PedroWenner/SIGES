function scrollRight(id) {
    const container = document.getElementById(id);
    container.scrollLeft += container.offsetWidth / 2;
    updateButtonVisibility(id);
}
function navegarLeft(id) {
    const container = document.getElementById(id);
    container.scrollLeft -= container.offsetWidth / 2;
    updateButtonVisibility(id);
}

// Função para verificar se há espaço para rolar e atualizar o estado do botão
function updateButtonVisibility(id) {
    const container = document.getElementById(id);
    const leftButton = container.parentElement.querySelector('.carousel-button.left');
    const rightButton = container.parentElement.querySelector('.carousel-button.right');

    // Verifica se o botão "esquerda" deve estar ativo
    if (container.scrollLeft <= 0) {
        leftButton.classList.add('disabled');
    } else {
        leftButton.classList.remove('disabled');
    }

    // Verifica se o botão "direita" deve estar ativo
    if (container.scrollLeft + container.clientWidth >= container.scrollWidth) {
        rightButton.classList.add('disabled');
    } else {
        rightButton.classList.remove('disabled');
    }
}

// Inicializa a visibilidade dos botões ao carregar a página
document.addEventListener('DOMContentLoaded', () => {
    updateButtonVisibility('categorias-carousel');
    updateButtonVisibility('banners-carousel');
    updateButtonVisibility('famosos-carousel');

    // Adiciona listeners de rolagem para atualizar a visibilidade dos botões dinamicamente
    ['categorias-carousel', 'banners-carousel', 'famosos-carousel'].forEach(id => {
        const container = document.getElementById(id);
        container.addEventListener('scroll', () => updateButtonVisibility(id));
    });
});