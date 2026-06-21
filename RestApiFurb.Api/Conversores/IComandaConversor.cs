using RestApiFurb.Api.ViewModels;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Api.Conversores;

public interface IComandaConversor
{
    CriarComandaContrato ConverterParaContrato(CriarComandaViewModel viewModel);

    ComandaCriadaViewModel ConverterParaViewModel(ComandaCriadaContrato contrato);

    AtualizarComandaContrato ConvertarParaContrato(AtualizarComandaViewModel viewModel);
}