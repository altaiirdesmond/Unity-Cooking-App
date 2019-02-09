namespace Assets.Script.DatabaseModel {
    public class Food {
        public int FoodId { get; set; }

        public string FoodName { get; set; }

        public string Region { get; set; }

        public string IngredientsTranslated { get; set; }

        public string Ingredients { get; set; }

        public string InstructionTranslated { get; set; }

        public string Instruction { get; set; }

        public string Category { get; set; }

        public string TriviaTranslated { get; set; }

        public string Trivia { get; set; }

        public byte[] Image { get; set; }
    }
}
