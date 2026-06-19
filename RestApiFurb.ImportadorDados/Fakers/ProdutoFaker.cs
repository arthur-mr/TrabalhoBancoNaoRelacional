using Bogus;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.ImportadorDados.Fakers;

public static class ProdutoFaker
{
    public static List<Produto> GerarMassaDeDados(int quantidade)
    {
        var categorias = new[] { "Lanches", "Pizzas", "Bebidas", "Porcoes", "Pratos Feitos", "Sobremesas" };

        var lanches = new[] { "X-Burger", "X-Salada", "X-Bacon", "X-Tudo", "X-Egg", "X-Calabresa", "X-Picanha", "X-Costela", "Smash Burger", "Cheddar Melt", "Hambúrguer", "Cachorro Quente" };
        var compLanche = new[] { "Simples", "Duplo", "Triplo", "Especial", "da Casa", "Artesanal", "Gourmet", "Premium", "Supremo", "Turbo", "Prensado", "Combo" };

        var pizzas = new[] { "Pizza", "Pizza Broto", "Calzone", "Esfiha Aberta", "Focaccia" };
        var saboresPizza = new[] { "Calabresa", "Mussarela", "Portuguesa", "Quatro Queijos", "Frango com Catupiry", "Marguerita", "Bacon", "Peperoni", "Napolitana", "Baiana", "Rúcula com Tomate Seco", "Alho e Óleo" };

        var bebidas = new[] { "Refrigerante", "Suco", "Chopp", "Cerveja", "Água", "Energético", "Chá Gelado" };
        var marcasBebida = new[] { "Cola", "Guaraná", "Laranja", "Uva", "Limão", "Pilsen", "IPA", "com Gás", "sem Gás" };
        var tamanhosBebida = new[] { "Lata 350ml", "Garrafa 600ml", "1 Litro", "2 Litros", "500ml", "Long Neck" };

        var porcoes = new[] { "Fritas", "Polenta", "Mandioca", "Calabresa Acebolada", "Anéis de Cebola", "Iscas de Peixe", "Iscas de Frango", "Bolinho de Queijo", "Coxinha" };
        var compPorcao = new[] { "Pequena", "Média", "Grande", "Inteira", "Meia", "com Bacon", "com Cheddar", "com Queijo Derretido", "ao Alho" };

        var pratos = new[] { "Prato Feito", "Marmitex", "Bife a Cavalo", "Estrogonofe", "Parmegiana", "Feijoada", "Macarronada" };
        var compPrato = new[] { "de Frango", "de Carne", "Misto", "Vegano", "Especial", "Tamanho P", "Tamanho G" };

        var sobremesas = new[] { "Pudim", "Sorvete", "Petit Gâteau", "Brownie", "Torta", "Mousse", "Cheesecake" };
        var saboresSobremesa = new[] { "de Leite Ninho", "de Chocolate", "de Morango", "de Limão", "de Maracujá", "de Doce de Leite", "de Nutella" };

        var faker = new Faker("pt_BR");
        var random = new Random();

        var nomesGerados = new HashSet<string>(quantidade);
        var produtos = new List<Produto>(quantidade);

        for (int i = 0; i < quantidade; i++)
        {
            string nomeProduto = string.Empty;
            string categoria = string.Empty;
            decimal precoDecimal = 0;

            bool unico = false;
            int tentativas = 0;

            while (!unico)
            {
                int catIndex = random.Next(categorias.Length);
                categoria = categorias[catIndex];

                switch (categoria)
                {
                    case "Lanches":
                        nomeProduto = $"{lanches[random.Next(lanches.Length)]} {compLanche[random.Next(compLanche.Length)]}";
                        precoDecimal = random.Next(25, 80) + (decimal)random.NextDouble();
                        break;
                    case "Pizzas":
                        nomeProduto = $"{pizzas[random.Next(pizzas.Length)]} de {saboresPizza[random.Next(saboresPizza.Length)]}";
                        precoDecimal = random.Next(50, 150) + (decimal)random.NextDouble();
                        break;
                    case "Bebidas":
                        nomeProduto = $"{bebidas[random.Next(bebidas.Length)]} {marcasBebida[random.Next(marcasBebida.Length)]} {tamanhosBebida[random.Next(tamanhosBebida.Length)]}";
                        precoDecimal = random.Next(5, 30) + (decimal)random.NextDouble();
                        break;
                    case "Porcoes":
                        nomeProduto = $"Porção de {porcoes[random.Next(porcoes.Length)]} {compPorcao[random.Next(compPorcao.Length)]}";
                        precoDecimal = random.Next(30, 100) + (decimal)random.NextDouble();
                        break;
                    case "Pratos Feitos":
                        nomeProduto = $"{pratos[random.Next(pratos.Length)]} {compPrato[random.Next(compPrato.Length)]}";
                        precoDecimal = random.Next(25, 70) + (decimal)random.NextDouble();
                        break;
                    case "Sobremesas":
                        nomeProduto = $"{sobremesas[random.Next(sobremesas.Length)]} {saboresSobremesa[random.Next(saboresSobremesa.Length)]}";
                        precoDecimal = random.Next(15, 45) + (decimal)random.NextDouble();
                        break;
                }

                if (tentativas > 3)
                {
                    nomeProduto += $" {random.Next(1, 99999)}";
                }

                unico = nomesGerados.Add(nomeProduto);
                tentativas++;
            }

            var produto = new Produto(
                codigo: faker.Random.AlphaNumeric(8).ToUpper(),
                nome: nomeProduto,
                codigoBarras: faker.Commerce.Ean13(),
                categoria: categoria,
                preco: Math.Round(precoDecimal, 2),
                quantidadeEstoque: random.Next(0, 500)
            );

            produtos.Add(produto);
        }

        return produtos;
    }
}