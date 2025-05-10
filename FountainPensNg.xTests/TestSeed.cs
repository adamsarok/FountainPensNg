namespace FountainPensNg.xTests;
public static class TestSeed {
	public static List<FountainPen> FountainPens = new List<FountainPen> {
			new FountainPen { Id = 0, Maker = "Maker1", ModelName = "Model1", Color = "#085172", Nib = "F", Rating = 10
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Model1")
			},
			new FountainPen { Id = 2, Maker = "Maker2", ModelName = "Model2", Color = "#d00606", Nib = "M", Rating = 5
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Model2")
			}
		};
	public static List<Ink> Inks = new List<Ink> {
			new Ink { Id = 0, Maker = "Maker1", InkName = "Ink1", Color = "#085172", Rating = 10
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Ink1")
			},
			new Ink { Id = 0, Maker = "Maker2", InkName = "Ink2", Color = "#d00606", Rating = 5
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Ink2")
			}
		};
	public static List<Paper> Papers = new List<Paper> {
			new Paper { Id = 0, Maker = "Maker1", PaperName = "Paper1", Rating = 10
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Paper1")
			},
			new Paper { Id = 0, Maker = "Maker2", PaperName = "Paper2", Rating = 5
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Paper2")
			}
		};
}