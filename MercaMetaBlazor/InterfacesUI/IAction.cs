using Microsoft.AspNetCore.Components;

namespace MercaMetaBlazor.InterfacesUI
{
    public interface IAction: IComponent
    {
        public void Mostrar();
        public void Dispose();
    }
}
