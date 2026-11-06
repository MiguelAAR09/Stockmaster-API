namespace StockMaster.Application.Excepciones;

// Otro proceso modificó los mismos datos entre la lectura y el guardado.
// No se guardó nada: la operación se puede reintentar con los datos actualizados.
public class ConflictoConcurrenciaException : Exception
{
    public ConflictoConcurrenciaException(string mensaje, Exception? causa = null)
        : base(mensaje, causa)
    {
    }
}
