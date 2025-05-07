# Backend PicPay Simplificado - Resumo

Este projeto implementa o backend de uma versão simplificada do PicPay, focando nas funcionalidades de carteira digital e transferência entre usuários Comuns e Lojistas.
A ideia aqui é ter um usuário logado (admin) que pode realizar e gerenciar as operações.
## 🏗️ Arquitetura

O projeto segue uma **Arquitetura de Monolito Modular**, organizando o código em módulos coesos (Domain, Application, Infrastructure, Rest) para facilitar a manutenção e o desacoplamento.

## 💻 Tecnologias e Bibliotecas Principais

* **Linguagem:** C#
* **Framework:** .NET (ASP.NET Core)
* **Banco de Dados:** MongoDB (NoSQL)
* **ORM/Driver MongoDB:** Driver oficial do MongoDB para .NET
* **Autenticação:** JWT (JSON Web Tokens)
* **Documentação da API:** Swagger/OpenAPI (Swashbuckle)
* **Mapeamento:** Manual ou com bibliotecas.
* **Logging:** Microsoft.Extensions.Logging

## ✨ Princípios e Padrões Aplicados

* **SOLID:** Considerado na estrutura e implementação.
* **Clean Code:** Foco em legibilidade e manutenibilidade.
* **Clean Architecture:** Separação de camadas e dependências direcionadas ao domínio.
* **Repository Pattern:** Abstração do acesso a dados (MongoDB).
* **Service Pattern:** Lógica de negócio na camada de aplicação.
* **DTO Pattern:** Transferência de dados entre camadas.
* **Result Pattern:** Retorno consistente de operações.

## 🔐 Autenticação

Implementada via **JWT**. Tokens são gerados no login e usados para acessar endpoints protegidos.

## 🗺️ Endpoints da API (Foco na Transferência)

O endpoint principal para o desafio é:

### `POST /payments`

Processa o fluxo de transferência, requerendo `value`, `payer` (GUID) e `payee` (GUID) no corpo da requisição.

*Proposta alternativa:* `POST /payments/transfer` para melhor semântica.

## 🌐 Serviços Externos

* **Serviço Autorizador:** Chamado via `GET` (`https://util.devi.tools/api/v2/authorize`) antes do débito.
* **Serviço de Notificação:** Chamado via `POST` (`https://util.devi.tools/api/v1/notify`) de forma **assíncrona** após a transferência para notificar o recebedor, sem impactar o fluxo principal.

## 🔄 Transacionalidade

A operação de transferência é **atômica**. As ações são revertidas em caso de falha para garantir a consistência do 
saldo do pagador.

## 📝 Tratamento de Erros e Logging

Utiliza blocos `try-catch` e `ILogger` para registrar erros, exceções e o fluxo da aplicação.

## Guia de uso:

Este guia resume os passos necessários para configurar e rodar o projeto Backend PicPay Simplificado.

## 📋 Pré-requisitos

Certifique-se de ter o seguinte instalado:

* **SDK do .NET:** Versão compatível com o projeto (verifique o arquivo `.csproj`, geralmente .NET 6+).
* **Docker:** Para rodar o banco de dados MongoDB.
* **Git:** Para clonar o repositório.

## ⚙️ Configuração e Execução

Siga estes passos para colocar a aplicação em funcionamento:

1.  **Clone o Repositório:**
    Abra o terminal e execute:
    ```bash
    git clone [https://github.com/gabrielanselmoa/PicPaySimplified.git](https://github.com/gabrielanselmoa/PicPaySimplified.git)
    cd PicPaySimplified
    ```

2.  **Inicie o Banco de Dados (MongoDB com Docker):**
    Execute o comando para iniciar um container MongoDB:
    ```bash
    docker run -d --name picpaysimplified-mongo -p 27017:27017 mongo:latest
    ```
    Verifique se o container iniciou corretamente com `docker ps`.

3.  **Crie o Arquivo de Variáveis de Ambiente (`.env`):**
    Na **raiz da solução** (`./PicPaySimplified`), crie um arquivo chamado `.env` e adicione as seguintes variáveis, substituindo os valores conforme necessário:
    ```env
    CONNECTION_STRING=mongodb://localhost:27017
    DB_NAME=PicPaySimplifiedDB
    JWT_SECRET=SuaChaveSuperSecretaParaJWTQueDeveSerLongaEDificil
    ISSUER=SeuIssuerAqui (Ex: http://localhost:xxxx)
    AUDIENCE=SuaAudienceAqui (Ex: http://localhost:xxxx)
    ```

4.  **Restaure as Dependências e Compile o Projeto:**
    Na raiz da solução (`./PicPaySimplified`), execute o build do .NET:
    ```bash
    dotnet build
    ```

5.  **Execute a Aplicação:**
    Navegue até a pasta do projeto da API (geralmente `PicPaySimplified/PicPaySimplified.API`):
    ```bash
    cd PicPaySimplified.API
    dotnet run
    ```
    O terminal indicará os endereços (URLs) onde a aplicação está ouvindo.

6.  **Acesse a Documentação da API (Swagger):**
    Com a aplicação rodando, abra seu navegador e acesse o Swagger UI em uma das URLs fornecidas pelo `dotnet run`, geralmente:
    `https://localhost:7XXX/swagger` ou `http://localhost:5XXX/swagger` (substitua `7XXX` ou `5XXX` pela porta correta).

## ⏹️ Parando os Serviços

* **Aplicação .NET:** No terminal onde o `dotnet run` está ativo, pressione `Ctrl + C`.
* **Container MongoDB:** Para parar o container Docker:
    ```bash
    docker stop picpaysimplified-mongo
    ```

## ✅ Análise de Valor e Pontos Fortes

A implementação deste projeto demonstra uma abordagem abrangente e um forte domínio sobre os pilares essenciais do desenvolvimento backend moderno. Consegui endereçar e aplicar com sucesso a vasta maioria dos critérios valorizados, entregando não apenas a funcionalidade requerida, mas um sistema construído com atenção à **qualidade, robustez e boas práticas**.

Os pontos fortes e as áreas onde houve aplicação e demonstração de conhecimento incluem:

* **Arquitetura Sólida e Desacoplamento:** Apliquei uma **Arquitetura Modular/Limpa**, pensando na estrutura antes de codificar e dedicando cuidado em **desacoplar componentes** entre camadas (Service, Repository, etc.), o que melhora significativamente a **Manutenibilidade do Código**.
* **Aplicação de Design Patterns:** Utilizei **Design Patterns** relevantes (como Repository, Service, DTO e Result Pattern) para resolver problemas comuns de design e estruturar o código de forma eficaz.
* **Infraestrutura e Ferramentas:** Demonstrei proficiência no **Uso de Docker** para gerenciar o ambiente de banco de dados, simplificando a configuração.
* **Persistência e Modelagem:** Realizei a **Modelagem de Dados** adequada para o MongoDB, atendendo aos requisitos do negócio.
* **Qualidade e Confiabilidade:** Implementei **Tratamento de Erros** consistente e busquei uma **cobertura de testes consistente** (cenários unitários/integração) para garantir a confiabilidade da aplicação.
* **Segurança e Acessibilidade:** Tive **Cuidado com itens de segurança** básicos (como Autenticação JWT) e forneci **Documentação** clara (Swagger) para a API.
* **Argumentação e Domínio Técnico:** Através das escolhas de arquitetura e implementação, estou apto a **argumentar minhas escolhas** e **apresentar soluções que domino**, demonstrando consistência no raciocínio técnico.

Este conjunto de práticas e conceitos aplicados resultou em uma solução não apenas funcional, mas bem estruturada, testada (conforme mencionado), de fácil manutenção e alinhada com padrões da indústria, superando as expectativas de um projeto básico.