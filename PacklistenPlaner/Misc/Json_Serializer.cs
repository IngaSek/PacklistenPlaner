using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace PacklistenPlaner; 

public class Json_Serializer
{
    public static void SaveToFile(DataRepository repository, string filename)
    {
        try
        {
            var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true };
            string json = JsonSerializer.Serialize<DataRepository>(repository, options);
            File.WriteAllText(filename, json);
        }
        catch(Exception ex)
        {
            System.Windows.MessageBox.Show($"Fehler beim Speichern: {ex.Message}\n\nStackTrace: {ex.StackTrace}");
        }
    }

    public static void LoadFromFile(DataRepository repository, string filename)
    {
        if (!File.Exists(filename))
        {
            return;
        }
        
        try 
        {
            string json = File.ReadAllText(filename);
            var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve};
            DataRepository? result = JsonSerializer.Deserialize<DataRepository>(json, options);
            
            if(result == null)
            {
                return;
            }

            if (result.Reisen?.Elements == null || !result.Reisen.Elements.Any())
            {
                return;
            }
            
            repository.Clear();

            if(result.Reisen?.Elements != null)
            {
                int count = 0;
                foreach (var reise in result.Reisen.Elements)
                {
                    repository.Reisen.Save(reise);
                    count++;
                }
            }

            if (result.Personen?.Elements != null)
            {
                foreach (var person in result.Personen.Elements)
                                repository.Personen.Save(person);
            }

            if (result.Vorlagen?.Elements != null)
            {
                foreach (var vorlage in result.Vorlagen.Elements)
                                repository.Vorlagen.Save(vorlage);
            }

            if (result.Gegenstaende?.Elements != null)
            {
                foreach (var gegenstand in result.Gegenstaende.Elements)
                    repository.Gegenstaende.Save(gegenstand);
            }
                
        }
        catch(Exception ex)
        {
            System.Windows.MessageBox.Show($"Fehler beim Laden: {ex.Message}");
        }

    }
}