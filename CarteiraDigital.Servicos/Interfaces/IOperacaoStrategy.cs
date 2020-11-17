namespace CarteiraDigital.Servicos
{
    public interface IOperacaoStrategy<TDtoEfetivar, TDtoGerar> where TDtoEfetivar : struct
                                                                where TDtoGerar : struct
    {
        void Efetivar(TDtoEfetivar operacao);
        void Gerar(TDtoGerar dto);
    }
}
