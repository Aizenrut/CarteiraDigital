namespace CarteiraDigital.Servicos
{
    public interface IOperacaoStrategy<TDto> where TDto : struct
    {
        void Efetivar(TDto operacaoDto);
    }
}
