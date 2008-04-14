using System;
using System.Collections.Generic;
using System.Text;

namespace Configuration
{
    public delegate void dUpdate();

    public class ConfiguratedByForm
    {
        public ConfiguratedByForm(object o)
        {
            CForm_ShowDialog cform = new CForm_ShowDialog();
            cform.SelectConfiguratoinObject(o);
            cform.ShowDialog();
        }

        public ConfiguratedByForm(object o, dUpdate update)
        {
            CForm_Show cform = new CForm_Show(update);
            cform.SelectConfiguratoinObject(o);
            cform.Show();
        }
    }
}
