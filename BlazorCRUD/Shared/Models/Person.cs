using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace BlazorCRUD.Shared
{
	public enum PersonOrdering : byte
	{
		NameA,
		NameD,
		AgeA,
		AgeD,
		SexA,
		SexD,
		HairColorA,
		HairColorD,
		None
	};

	public enum HairColor : byte
	{
		Black, DarkBrown, Brown, 
		Auburn, Ginger,
		DarkBlond, Blond, LightBlond
	}

	public class Person
	{
		public int? Id { get; set; }
		public string? Name { get; set; }
		public short? Age { get; set; }
		public bool? Sex { get; set; }
		public HairColor? HairColor { get; set; }

		public Person() { }
		//public Person(string json) : this(JsonSerializer.Deserialize<Person>(json)!) { }
		public Person(string serialized)
		{
			var fields = Regex.Split(serialized, "(?<!\\\\)[,]", new(), TimeSpan.FromMilliseconds(500));
			if (fields.Length != 5) throw new ArgumentException($"Unable to deserialize string {nameof(serialized)}");
			Id = int.TryParse(fields[0], out int id) ? id : null;
			Name = fields[1] != ""? fields[1].Replace("\\,", ",") : null;
			Age = short.TryParse(fields[2], out short age) ? age : null;
			Sex = bool.TryParse(fields[3], out bool sex) ? sex : null;
			HairColor = byte.TryParse(fields[4], out byte hairColor) ? (HairColor)hairColor : null;
		}
		public Person(Person copy)
		{
			Id = copy.Id;
			Name = copy.Name;
			Age = copy.Age;
			Sex = copy.Sex;
			HairColor = copy.HairColor;
		}
		public override string ToString()
		{
			return JsonSerializer.Serialize(
				this, new JsonSerializerOptions()
				{
					DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
				});
		}
		public string Serialize()
		{
			return $"{Id},{Name?.Replace(",", "\\,") ?? ""},{Age},{Sex},{(byte?)HairColor}";
		}
		public bool IsEmpty()
		{
			return Id == null && Name == null && Age == null && Sex == null && HairColor == null;
		}
	}
}