using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class Extensions
{
    public static string Format<T>(this List<T> me)
    {
        return string.Join(", ", me);
    }
    public static bool Accept(this DialogResult me)
    {
        return me == DialogResult.OK || me == DialogResult.Yes;
    }

}

