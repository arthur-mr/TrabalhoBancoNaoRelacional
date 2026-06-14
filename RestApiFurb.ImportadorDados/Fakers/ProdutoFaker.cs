namespace RestApiFurb.ImportadorDados.Fakers;

using Bogus;
using RestApiFurb.Dominio.Modelos;
using System;
using System.Collections.Generic;

public static class ProdutoFaker
{
    public static List<Produto> GerarMassaDeDados(int quantidade)
    {
        var faker = new Faker<Produto>("pt_BR")
            .CustomInstantiator(f =>
            {
                var produto = new Produto(
                    codigo: f.Random.AlphaNumeric(8).ToUpper(),
                    nome: f.Commerce.ProductName(),
                    codigoBarras: f.Commerce.Ean13(),
                    categoria: f.Commerce.Department(),
                    preco: Math.Round(f.Random.Decimal(10m, 5000m), 2),
                    quantidadeEstoque: f.Random.Int(0, 1000)
                );

                return produto;
            });

        return faker.Generate(quantidade);
    }
}