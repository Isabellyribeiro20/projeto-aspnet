# 🐾 Patudinhos

Loja virtual para produtos pet desenvolvida com **ASP.NET Core MVC**, **C#** e **Entity Framework Core**.

---

## Sobre o projeto

O Patudinhos é uma aplicação web de e-commerce voltada para produtos de pet shop. O sistema conta com dois níveis de acesso — administrador e cliente — e permite o cadastro de produtos, navegação por catálogo, carrinho de compras e registro de pedidos.

Projeto desenvolvido para a disciplina de **Técnicas de Programação**.

---

## Tecnologias utilizadas

| Tecnologia | Versão | Uso |
|---|---|---|
| ASP.NET Core MVC | 10.0 | Framework web |
| C# | 13 | Linguagem de programação |
| Entity Framework Core | 10.0 | ORM e acesso ao banco |
| ASP.NET Core Identity | 10.0 | Autenticação e autorização |
| SQLite | — | Banco de dados |
| Razor / CSHTML | — | Views e templates |

---

## Níveis de acesso

### Administrador
- Credenciais criadas automaticamente na primeira execução
- **Email:** `admin@patudinhos.com`
- **Senha:** `Admin123!`
- Permissões: cadastrar, editar e excluir produtos; visualizar todos os pedidos

### Cliente
- Cadastro livre pela tela de registro
- Permissões: navegar pelo catálogo, adicionar ao carrinho, finalizar pedidos e visualizar histórico de compras

---

## Como executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local ou remoto)

### Passo a passo

**1. Clone o repositório**
```bash
git clone https://github.com/seu-usuario/patudinhos.git
cd patudinhos
```

**2. Configure a connection string**

Abra o `appsettings.json` e ajuste a string de conexão:
```json
{
  "ConnectionStrings": {
    "AppDbContextConnection": "Server=localhost;Database=Patudinhos;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**3. Aplique as migrations**
```bash
dotnet ef database update
```

**4. Execute o projeto**
```bash
dotnet run
```

**5. Acesse no navegador**
```
http://localhost:5272
```

O usuário administrador é criado automaticamente na primeira execução.

---

## Principais funcionalidades

### Catálogo de produtos
- Listagem em grid com foto, nome, categoria, preço e estoque
- Filtro por categoria
- Botão "Adicionar ao carrinho" disponível para itens em estoque

### Carrinho de compras
- Armazenado em sessão — requer login para montar
- Ajuste de quantidade por item
- Remoção individual de itens
- Resumo com total e botão de finalização

### Pedidos
- Criados automaticamente ao finalizar o carrinho
- Histórico de pedidos por usuário com status e itens detalhados
- Admin visualiza todos os pedidos com opções de edição

### Gerenciamento de produtos (admin)
- Upload de foto com preview antes de salvar
- Controle de estoque
- CRUD completo com validações

---

## Segurança

- Senhas armazenadas com hash via ASP.NET Core Identity
- Rotas administrativas protegidas com `[Authorize(Roles = "Admin")]`
- Proteção contra CSRF em todos os formulários via `[ValidateAntiForgeryToken]`
- Usuário comum não consegue acessar rotas de admin mesmo digitando a URL manualmente

---

## Desenvolvido por

Gleice Constâncio Rodrigues e Isabelly Ribeiro, alunas do 2° semestre de Desenvolvimento de Software Multiplataforma da FATEC Olímpia.
