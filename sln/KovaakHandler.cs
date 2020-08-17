using GvasFormat.Serialization.UETypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KovaakShuffler
{
    public class KovaakHandler
    {
        public List<Tuple<string, int>> ImportPlaylist(UEProperty[] scenarioList)
        {
            var listedScenarios = new List<Tuple<string, int>>();


            foreach (var scenarioItem in scenarioList)
            {
                var scenarioProp = (UEGenericStructProperty)scenarioItem;

                Console.WriteLine($"Scenario = {scenarioProp.Header} {scenarioProp.Name} {scenarioProp.Type} {scenarioProp.StructType} {scenarioProp.Properties.Count}");

                var name = (UEStringProperty)scenarioProp.Properties[0];
                var count = (UEIntProperty)scenarioProp.Properties[1];
                listedScenarios.Add(new Tuple<string, int>(name.Value, count.Value));
            }

            return listedScenarios;
        }

        public UEProperty[] ExportPlaylist(List<string> scenariosList, UEGenericStructProperty scenarioItem)
        {
            var sampleName = (UEStringProperty)scenarioItem.Properties[0];
            var sampleCount = (UEIntProperty)scenarioItem.Properties[1];
            var sampleNone = (UENoneProperty)scenarioItem.Properties[2];

            var proparr = new List<UEProperty>();

            foreach (var scene in scenariosList)
            {
                var name = new UEStringProperty();
                var count = new UEIntProperty();
                var nil = new UENoneProperty();

                name.Value = scene;
                name.Name = sampleName.Name;
                name.Type = sampleName.Type;

                count.Value = 1;
                count.Name = sampleCount.Name;
                count.Type = sampleCount.Type;

                var scenarioProp = new UEGenericStructProperty();
                scenarioProp.Properties = new List<UEProperty>();
                scenarioProp.Name = string.Copy(scenarioItem.Name);
                scenarioProp.Type = string.Copy(scenarioItem.Type);
                scenarioProp.Header = string.Copy(scenarioItem.Header);
                //scenarioProp.StructType = string.Copy(scenarioItem.StructType);



                scenarioProp.Properties.Add(name);
                scenarioProp.Properties.Add(count);
                scenarioProp.Properties.Add(sampleNone);
                proparr.Add(scenarioProp);

            }

            return proparr.ToArray();
        }

        public UEProperty[] ExportPlaylist(List<Tuple<string, int>> playlist, UEGenericStructProperty scenarioItem)
        {
            var sampleName = (UEStringProperty)scenarioItem.Properties[0];
            var sampleCount = (UEIntProperty)scenarioItem.Properties[1];
            var sampleNone = (UENoneProperty)scenarioItem.Properties[2];

            var proparr = new List<UEProperty>();

            foreach (var pair in playlist)
            {
                var name = new UEStringProperty();
                var count = new UEIntProperty();
                var nil = new UENoneProperty();

                name.Value = pair.Item1;
                name.Name = sampleName.Name;
                name.Type = sampleName.Type;

                count.Value = pair.Item2;
                count.Name = sampleCount.Name;
                count.Type = sampleCount.Type;

                var scenarioProp = new UEGenericStructProperty();
                scenarioProp.Properties = new List<UEProperty>();
                scenarioProp.Name = string.Copy(scenarioItem.Name);
                scenarioProp.Type = string.Copy(scenarioItem.Type);
                scenarioProp.Header = string.Copy(scenarioItem.Header);
                //scenarioProp.StructType = string.Copy(scenarioItem.StructType);


                scenarioProp.Properties.Add(name);
                scenarioProp.Properties.Add(count);
                scenarioProp.Properties.Add(sampleNone);
                proparr.Add(scenarioProp);

            }

            return proparr.ToArray();
        }
    }
}
