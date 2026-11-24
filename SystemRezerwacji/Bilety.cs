using System;
using System.Collections.Generic;
using System.Text;
using SystemRezerwacji;

namespace SystemRezerwacji
{
    public interface IPodsumowanie
    {
        string PobierzOpis();
    }

    public abstract class Bilet : IPodsumowanie
    {
        public int Rzad { get; set; }

        public int Miejsce { get; set; }
        public decimal CenaBazowa { get; set; }
        protected Bilet(int rzad, int miejsce, decimal cenaBazowa)
        {
            Rzad = rzad;
            Miejsce = miejsce;
            CenaBazowa = cenaBazowa;
        }

        public abstract decimal ObliczCene();
        public virtual string PobierzOpis()
        {
            return $"Rzad: {Rzad}, Miejsce: {Miejsce}, Cena: {ObliczCene()}zl";
        }
    }
    public class BiletNormalny : Bilet
    {
        public BiletNormalny(int rzad, int miejsce, decimal cenaBazowa) : base(rzad, miejsce, cenaBazowa)
        {
        }
    public override decimal ObliczCene()
        {
            return CenaBazowa;
        }

        public override string PobierzOpis()
        {
            return $"Normalny {base.PobierzOpis()}";
        }
    }

    public class BiletUlgowy : Bilet
    {

    public BiletUlgowy(int rzad, int miejsce, decimal cenaBazowa) : base(rzad, miejsce, cenaBazowa)
    {
    }

    public override decimal ObliczCene()
    {
        return CenaBazowa * 0.5m;
    }

    public override string PobierzOpis()
    {
        return $"Ulgowy {base.PobierzOpis()}";
    }
}
}
