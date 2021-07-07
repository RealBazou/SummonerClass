using SolastaModApi;
using SolastaModApi.Extensions;

namespace SummonerClass
{

    internal class RubikusMonsterBuilder : BaseDefinitionBuilder<MonsterDefinition>
    {
        const string RubikusMonsterName = "Rubikus";
        const string RubikusMonsterNameGuid = "0a3e6a7d-4628-4189-b91d-d7146d771234";

        protected RubikusMonsterBuilder(string name, string guid) : base(DatabaseHelper.MonsterDefinitions.CubeOfLight, name, guid)
        {
            Definition.GuiPresentation.Title = "Rubikus";
            Definition.GuiPresentation.Description = "Fate has granted you a portion of its power through this construct.";

            int[] abilityScores = new int[] { 20, 20, 20, 20, 20, 20 };
            Definition.SetAbilityScores(abilityScores);
            Definition.SetAlignment("neutral");
            Definition.SetArmorClass(20);
            Definition.SizeDefinition = DatabaseHelper.CharacterSizeDefinitions.Small;
            Definition.SetDefaultBattleDecisionPackage(DatabaseHelper.DecisionPackageDefinitions.DefaultSupportCasterWithBackupAttacksDecisions);
            Definition.SetInDungeonEditor(true);
            Definition.SetFullyControlledWhenAllied(true);
            Definition.SetDefaultFaction("Party");
        }

        public static MonsterDefinition CreateAndAddToDB(string name, string guid)
            => new RubikusMonsterBuilder(name, guid).AddToDB();

        public static MonsterDefinition RubikusMonster = CreateAndAddToDB(RubikusMonsterName, RubikusMonsterNameGuid);

        public static MonsterDefinition AddToMonsterList()
        {
            var RubikusMonster = RubikusMonsterBuilder.RubikusMonster;//Instantiating it adds to the DB
            return RubikusMonster;
        }

    }

}