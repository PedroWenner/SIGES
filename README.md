# SIGES — Sistema Integrado de Gestão

Sistema web de gestão para pequenas empresas e assistências técnicas: clientes, produtos, serviços, ordens de serviço, vendas, financeiro e cobranças (PIX/boleto), com controle de permissões por perfil.

Desenvolvido em **ASP.NET Core 8 (MVC)** com **Entity Framework Core** e **MySQL**.

## Funcionalidades

- **Cadastros**: clientes, produtos, categorias, serviços, usuários e emitente
- **Ordens de Serviço**: status configuráveis, produtos e serviços por OS, anotações, anexos com miniaturas e garantia
- **Vendas**: itens de venda e geração de documentos em PDF
- **Financeiro**: lançamentos, contas e cobranças com integração a gateways de pagamento (Mercado Pago, EFI, Asaas) e geração de PIX com QR Code
- **Relatórios** em PDF (iText 7 / PuppeteerSharp)
- **Segurança**: autenticação por cookie, senhas com BCrypt, permissões por perfil via atributo + middleware, validação de CPF e log de alterações (auditoria)
- **E-mail**: fila de envio processada por um `BackgroundService`
- **API REST** para mensagens e PIX

## Arquitetura

```
Controllers/   Camada MVC e endpoints da API
Models/        Entidades de domínio
Repositorios/  Padrão Repository (interfaces em Interfaces/)
Services/      Regras de integração: e-mail, gateways, autenticação, temas
Middleware/    Verificação de permissões por requisição
BackgroundJobs Envio assíncrono da fila de e-mails
Migrations/    Migrations do EF Core
Views/         Razor views
```

## Tecnologias

ASP.NET Core 8 · EF Core 8 (Pomelo MySQL) · Razor · Bootstrap / MDB · DataTables · BCrypt.Net · iText 7 · PuppeteerSharp · Mercado Pago SDK · QRCoder

## Como executar

Pré-requisitos: [.NET SDK 8](https://dotnet.microsoft.com/download) e MySQL 8.

```bash
git clone https://github.com/PedroWenner/SIGES.git
cd SIGES

# 1. Configure segredos fora do repositório (User Secrets)
dotnet user-secrets set "ConnectionStrings:MySql" "Server=localhost;Database=SIGES;user=root;password=SUA_SENHA;"
# opcionais: e-mail e gateways
dotnet user-secrets set "AppSettings:EMAIL_SMTP_USER" "seu@email.com"
dotnet user-secrets set "AppSettings:EMAIL_SMTP_PASS" "senha-de-app"

# 2. Crie o banco
dotnet tool install --global dotnet-ef
dotnet ef database update

# 3. Rode
dotnet run
```

Acesse `http://localhost:5229`. O seed cria o usuário inicial **teste@teste.com** / **123** — altere a senha após o primeiro acesso.

> O `appsettings.json` contém apenas valores de exemplo. Nunca versione credenciais reais.

## Licença

Distribuído sob a licença MIT — veja [LICENSE](LICENSE).
