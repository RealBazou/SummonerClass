using SolastaModApi;
using SolastaModApi.Extensions;
using SolastaModHelpers;
using System.Collections.Generic;
using System.Linq;

using Helpers = SolastaModHelpers.Helpers;
using NewFeatureDefinitions = SolastaModHelpers.NewFeatureDefinitions;
using ExtendedEnums = SolastaModHelpers.ExtendedEnums;

namespace SummonerClass
{
    internal class SummonerClassBuilder : CharacterClassDefinitionBuilder
    {
        const string SummonerClassName = "SummonerClass";
        const string SummonerClassNameGuid = "274106b8-0376-4bcd-bd1b-440633a31234";
        const string SummonerClassSubclassesGuid = "be865126-d7c3-45f3-b891-e77bd8b01234";

        static public CharacterClassDefinition summoner_class;
        static public SpellListDefinition summoner_spelllist;

        // Rubikus Path
        static public FeatureDefinitionPower summon_rubikus;

        protected SummonerClassBuilder(string name, string guid) : base(name, guid)
        {
            var summoner_class_image = SolastaModHelpers.CustomIcons.Tools.storeCustomIcon("SummonerClassImage",
                                                                                           $@"{UnityModManagerNet.UnityModManager.modsPath}/SummonerClass/Sprites/SummonerClass.png",
                                                                                           860, 858);
            //                                                                                           1024, 576);
            var wizard = DatabaseHelper.CharacterClassDefinitions.Wizard;
            summoner_class = Definition;
            Definition.GuiPresentation.Title = "Class/&SummonerClassTitle";
            Definition.GuiPresentation.Description = "Class/&SummonerClassDescription";
            Definition.GuiPresentation.SetSpriteReference(summoner_class_image);

            Definition.SetClassAnimationId(AnimationDefinitions.ClassAnimationId.Wizard);
            Definition.SetClassPictogramReference(wizard.ClassPictogramReference);
            Definition.SetDefaultBattleDecisions(wizard.DefaultBattleDecisions);
            Definition.SetHitDice(RuleDefinitions.DieType.D6);
            Definition.SetIngredientGatheringOdds(wizard.IngredientGatheringOdds);
            Definition.SetRequiresDeity(false);

            Definition.AbilityScoresPriority.Clear();
            Definition.AbilityScoresPriority.AddRange(new List<string> {Helpers.Stats.Charisma,
                                                                        Helpers.Stats.Wisdom,
                                                                        Helpers.Stats.Intelligence,
                                                                        Helpers.Stats.Constitution,
                                                                        Helpers.Stats.Dexterity,
                                                                        Helpers.Stats.Strength});

            Definition.FeatAutolearnPreference.AddRange(wizard.FeatAutolearnPreference);
            Definition.PersonalityFlagOccurences.AddRange(wizard.PersonalityFlagOccurences);

            Definition.SkillAutolearnPreference.Clear();
            Definition.SkillAutolearnPreference.AddRange(new List<string> { Helpers.Skills.Arcana,
                                                                            Helpers.Skills.Persuasion,
                                                                            Helpers.Skills.Deception,
                                                                            Helpers.Skills.Intimidation,
                                                                            Helpers.Skills.Insight,
                                                                            Helpers.Skills.History });

            Definition.ToolAutolearnPreference.Clear();
            Definition.ToolAutolearnPreference.AddRange(new List<string> { Helpers.Tools.EnchantingTool, Helpers.Tools.HerbalismKit });


            Definition.EquipmentRows.AddRange(wizard.EquipmentRows);
            Definition.EquipmentRows.Clear();

            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Dagger, EquipmentDefinitions.OptionWeapon, 1),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Quarterstaff, EquipmentDefinitions.OptionWeapon, 1),
                                    }
            );
            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ExplorerPack, EquipmentDefinitions.OptionStarterPack, 1),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ScholarPack, EquipmentDefinitions.OptionStarterPack, 1),
                                    }
            );
            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.EnchantingTool, EquipmentDefinitions.OptionTool, 1),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                         EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.HerbalismKit, EquipmentDefinitions.OptionTool, 1),
                                    }
            );

            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
            {
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ClothesWizard, EquipmentDefinitions.OptionArmor, 1),
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ComponentPouch, EquipmentDefinitions.OptionFocus, 1)
            });

            var saving_throws = Helpers.ProficiencyBuilder.CreateSavingthrowProficiency("SummonerSavingthrowProficiency",
                                                                                        "88d8752b-4956-4daf-91fc-84e6196c1234",
                                                                                        Helpers.Stats.Charisma, Helpers.Stats.Wisdom);

            var weapon_proficiency = Helpers.ProficiencyBuilder.createCopy("SummonerWeaponProficiency",
                                                                          "9a0ef52f-052a-4838-b3d4-2096ab671234",
                                                                          "Feature/&SummonerWeaponProficiencyTitle",
                                                                          "",
                                                                          DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyWizardWeapon
                                                                          );

            var tools_proficiency = Helpers.ProficiencyBuilder.CreateToolsProficiency("SummonerToolsProficiency",
                                                                                      "96d8987b-e682-44a6-afdb-763cbe531234",
                                                                                      "Feature/&SummonerToolsProficiencyTitle",
                                                                                      Helpers.Tools.EnchantingTool, Helpers.Tools.HerbalismKit
                                                                                      );

            var skills = Helpers.PoolBuilder.createSkillProficiency("SummonerSkillProficiency",
                                                                    "029f6c7e-f1fc-4030-9012-9c698c711234",
                                                                    "Feature/&SummonerClassSkillPointPoolTitle",
                                                                    "Feature/&SummonerClassSkillPointPoolDescription",
                                                                    2,
                                                                    Helpers.Skills.getAllSkills());

            var ritual_spellcasting = Helpers.RitualSpellcastingBuilder.createRitualSpellcasting("SummonerRitualSpellcasting",
                                                                                                 "25c48b9b-e2e9-4ea7-8a80-e6c413271234",
                                                                                                 "Feature/&SummonerClassRitualCastingDescription",
                                                                                                 (RuleDefinitions.RitualCasting)ExtendedEnums.ExtraRitualCasting.Spontaneous);

            summoner_spelllist = Helpers.SpelllistBuilder.create9LevelSpelllist("SummonerClassSpelllist", "0f3d14a7-f9a1-41ec-a164-f3e0f3801234", "",
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.AcidSplash,
                                                                                    DatabaseHelper.SpellDefinitions.AnnoyingBee,
                                                                                    DatabaseHelper.SpellDefinitions.ChillTouch,
                                                                                    DatabaseHelper.SpellDefinitions.DancingLights,
                                                                                    DatabaseHelper.SpellDefinitions.Dazzle,
                                                                                    DatabaseHelper.SpellDefinitions.FireBolt,
                                                                                    DatabaseHelper.SpellDefinitions.Guidance,
                                                                                    DatabaseHelper.SpellDefinitions.Light,
                                                                                    DatabaseHelper.SpellDefinitions.PoisonSpray,
                                                                                    DatabaseHelper.SpellDefinitions.RayOfFrost,
                                                                                    DatabaseHelper.SpellDefinitions.Resistance,
                                                                                    DatabaseHelper.SpellDefinitions.SacredFlame,
                                                                                    DatabaseHelper.SpellDefinitions.ShadowArmor,
                                                                                    DatabaseHelper.SpellDefinitions.ShadowDagger,
                                                                                    DatabaseHelper.SpellDefinitions.Shine,
                                                                                    DatabaseHelper.SpellDefinitions.SpareTheDying,
                                                                                    DatabaseHelper.SpellDefinitions.Sparkle,
                                                                                    DatabaseHelper.SpellDefinitions.TrueStrike
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.AnimalFriendship,
                                                                                    DatabaseHelper.SpellDefinitions.Bane,
                                                                                    DatabaseHelper.SpellDefinitions.Bless,
                                                                                    DatabaseHelper.SpellDefinitions.BurningHands,
                                                                                    DatabaseHelper.SpellDefinitions.CharmPerson,
                                                                                    DatabaseHelper.SpellDefinitions.ColorSpray,
                                                                                    DatabaseHelper.SpellDefinitions.ComprehendLanguages,
                                                                                    DatabaseHelper.SpellDefinitions.CureWounds,
                                                                                    DatabaseHelper.SpellDefinitions.DetectEvilAndGood,
                                                                                    DatabaseHelper.SpellDefinitions.DetectMagic,
                                                                                    DatabaseHelper.SpellDefinitions.DetectPoisonAndDisease,
                                                                                    DatabaseHelper.SpellDefinitions.DivineFavor,
                                                                                    DatabaseHelper.SpellDefinitions.Entangle,
                                                                                    DatabaseHelper.SpellDefinitions.ExpeditiousRetreat,
                                                                                    DatabaseHelper.SpellDefinitions.FaerieFire,
                                                                                    DatabaseHelper.SpellDefinitions.FalseLife,
                                                                                    DatabaseHelper.SpellDefinitions.FeatherFall,
                                                                                    DatabaseHelper.SpellDefinitions.FogCloud,
                                                                                    DatabaseHelper.SpellDefinitions.Goodberry,
                                                                                    DatabaseHelper.SpellDefinitions.Grease,
                                                                                    DatabaseHelper.SpellDefinitions.GuidingBolt,
                                                                                    DatabaseHelper.SpellDefinitions.HealingWord,
                                                                                    DatabaseHelper.SpellDefinitions.Heroism,
                                                                                    DatabaseHelper.SpellDefinitions.HideousLaughter,
                                                                                    DatabaseHelper.SpellDefinitions.HuntersMark,
                                                                                    DatabaseHelper.SpellDefinitions.Identify,
                                                                                    DatabaseHelper.SpellDefinitions.InflictWounds,
                                                                                    DatabaseHelper.SpellDefinitions.Jump,
                                                                                    DatabaseHelper.SpellDefinitions.Longstrider,
                                                                                    DatabaseHelper.SpellDefinitions.MageArmor,
                                                                                    DatabaseHelper.SpellDefinitions.MagicMissile,
                                                                                    DatabaseHelper.SpellDefinitions.ProtectionFromEvilGood,
                                                                                    DatabaseHelper.SpellDefinitions.Shield,
                                                                                    DatabaseHelper.SpellDefinitions.ShieldOfFaith,
                                                                                    DatabaseHelper.SpellDefinitions.Sleep,
                                                                                    DatabaseHelper.SpellDefinitions.Thunderwave
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.AcidArrow,
                                                                                    DatabaseHelper.SpellDefinitions.Aid,
                                                                                    DatabaseHelper.SpellDefinitions.Barkskin,
                                                                                    DatabaseHelper.SpellDefinitions.Blindness,
                                                                                    DatabaseHelper.SpellDefinitions.Blur,
                                                                                    DatabaseHelper.SpellDefinitions.BrandingSmite,
                                                                                    DatabaseHelper.SpellDefinitions.CalmEmotions,
                                                                                    DatabaseHelper.SpellDefinitions.ConjureGoblinoids,
                                                                                    DatabaseHelper.SpellDefinitions.Darkness,
                                                                                    DatabaseHelper.SpellDefinitions.Darkvision,
                                                                                    DatabaseHelper.SpellDefinitions.EnhanceAbility,
                                                                                    DatabaseHelper.SpellDefinitions.FindTraps,
                                                                                    DatabaseHelper.SpellDefinitions.FlamingSphere,
                                                                                    DatabaseHelper.SpellDefinitions.GustOfWind,
                                                                                    DatabaseHelper.SpellDefinitions.HoldPerson,
                                                                                    DatabaseHelper.SpellDefinitions.Invisibility,
                                                                                    DatabaseHelper.SpellDefinitions.Knock,
                                                                                    DatabaseHelper.SpellDefinitions.LesserRestoration,
                                                                                    DatabaseHelper.SpellDefinitions.Levitate,
                                                                                    DatabaseHelper.SpellDefinitions.MagicWeapon,
                                                                                    DatabaseHelper.SpellDefinitions.MistyStep,
                                                                                    DatabaseHelper.SpellDefinitions.PassWithoutTrace,
                                                                                    DatabaseHelper.SpellDefinitions.PrayerOfHealing,
                                                                                    DatabaseHelper.SpellDefinitions.ProtectionFromPoison,
                                                                                    DatabaseHelper.SpellDefinitions.RayOfEnfeeblement,
                                                                                    DatabaseHelper.SpellDefinitions.ScorchingRay,
                                                                                    DatabaseHelper.SpellDefinitions.SeeInvisibility,
                                                                                    DatabaseHelper.SpellDefinitions.Shatter,
                                                                                    DatabaseHelper.SpellDefinitions.Silence,
                                                                                    DatabaseHelper.SpellDefinitions.SpiderClimb,
                                                                                    DatabaseHelper.SpellDefinitions.SpiritualWeapon,
                                                                                    DatabaseHelper.SpellDefinitions.WardingBond
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.BeaconOfHope,
                                                                                    DatabaseHelper.SpellDefinitions.BestowCurse,
                                                                                    DatabaseHelper.SpellDefinitions.ConjureAnimals,
                                                                                    DatabaseHelper.SpellDefinitions.Counterspell,
                                                                                    DatabaseHelper.SpellDefinitions.CreateFood,
                                                                                    DatabaseHelper.SpellDefinitions.Daylight,
                                                                                    DatabaseHelper.SpellDefinitions.DispelMagic,
                                                                                    DatabaseHelper.SpellDefinitions.Fear,
                                                                                    DatabaseHelper.SpellDefinitions.Fireball,
                                                                                    DatabaseHelper.SpellDefinitions.Fly,
                                                                                    DatabaseHelper.SpellDefinitions.Haste,
                                                                                    DatabaseHelper.SpellDefinitions.HypnoticPattern,
                                                                                    DatabaseHelper.SpellDefinitions.LightningBolt,
                                                                                    DatabaseHelper.SpellDefinitions.MassHealingWord,
                                                                                    DatabaseHelper.SpellDefinitions.ProtectionFromEnergy,
                                                                                    DatabaseHelper.SpellDefinitions.RemoveCurse,
                                                                                    DatabaseHelper.SpellDefinitions.Revivify,
                                                                                    DatabaseHelper.SpellDefinitions.SleetStorm,
                                                                                    DatabaseHelper.SpellDefinitions.Slow,
                                                                                    DatabaseHelper.SpellDefinitions.SpiritGuardians,
                                                                                    DatabaseHelper.SpellDefinitions.StinkingCloud,
                                                                                    DatabaseHelper.SpellDefinitions.Tongues,
                                                                                    DatabaseHelper.SpellDefinitions.VampiricTouchIntelligence,
                                                                                    DatabaseHelper.SpellDefinitions.WindWall
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.Banishment,
                                                                                    DatabaseHelper.SpellDefinitions.BlackTentacles,
                                                                                    DatabaseHelper.SpellDefinitions.Blight,
                                                                                    DatabaseHelper.SpellDefinitions.Confusion,
                                                                                    DatabaseHelper.SpellDefinitions.ConjureMinorElementals,
                                                                                    DatabaseHelper.SpellDefinitions.DeathWard,
                                                                                    DatabaseHelper.SpellDefinitions.DimensionDoor,
                                                                                    DatabaseHelper.SpellDefinitions.FireShield,
                                                                                    DatabaseHelper.SpellDefinitions.FreedomOfMovement,
                                                                                    DatabaseHelper.SpellDefinitions.GiantInsect,
                                                                                    DatabaseHelper.SpellDefinitions.GreaterInvisibility,
                                                                                    DatabaseHelper.SpellDefinitions.GuardianOfFaith,
                                                                                    DatabaseHelper.SpellDefinitions.IceStorm,
                                                                                    DatabaseHelper.SpellDefinitions.IdentifyCreatures,
                                                                                    DatabaseHelper.SpellDefinitions.PhantasmalKiller,
                                                                                    DatabaseHelper.SpellDefinitions.Stoneskin,
                                                                                    DatabaseHelper.SpellDefinitions.WallOfFire
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.CloudKill,
                                                                                    DatabaseHelper.SpellDefinitions.ConeOfCold,
                                                                                    DatabaseHelper.SpellDefinitions.ConjureElemental,
                                                                                    DatabaseHelper.SpellDefinitions.Contagion,
                                                                                    DatabaseHelper.SpellDefinitions.DispelEvilAndGood,
                                                                                    DatabaseHelper.SpellDefinitions.DominatePerson,
                                                                                    DatabaseHelper.SpellDefinitions.FlameStrike,
                                                                                    DatabaseHelper.SpellDefinitions.GreaterRestoration,
                                                                                    DatabaseHelper.SpellDefinitions.HoldMonster,
                                                                                    DatabaseHelper.SpellDefinitions.InsectPlague,
                                                                                    DatabaseHelper.SpellDefinitions.MassCureWounds,
                                                                                    DatabaseHelper.SpellDefinitions.MindTwist,
                                                                                    DatabaseHelper.SpellDefinitions.RaiseDead
                                                                                }
                                                                                );

            var summoner_spellcasting = Helpers.SpellcastingBuilder.createSpontaneousSpellcasting("SummonerClassSpellcasting",
                                                                                              "f720edaf-92c4-43e3-8228-c48c0b411234",
                                                                                              "Feature/&SummonerClassSpellcastingTitle",
                                                                                              "Feature/&SummonerClassSpellcastingDescription",
                                                                                              summoner_spelllist,
                                                                                              Helpers.Stats.Charisma,
                                                                                              DatabaseHelper.FeatureDefinitionCastSpells.CastSpellWizard.KnownCantrips,
                                                                                              new List<int> { 4,  5,  6,  7,  8,  9, 10, 11, 12, 12,
                                                                                                             13, 13, 14, 14, 15, 15, 16, 16, 16, 16},
                                                                                              DatabaseHelper.FeatureDefinitionCastSpells.CastSpellWizard.SlotsPerLevels
                                                                                              );

//            createCountercharm();
            Definition.FeatureUnlocks.Clear();
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(saving_throws, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(weapon_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(skills, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(tools_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(summoner_spellcasting, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(ritual_spellcasting, 1));
//            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(song_of_rest[RuleDefinitions.DieType.D6], 2));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 4));
//            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(countercharm, 6));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 8));
//            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(magical_secrets, 10));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 12));
//            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(magical_secrets14, 14));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 16));
//            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(magical_secrets18, 18));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 19));
//            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(great_exodus, 20));

            var subclassChoicesGuiPresentation = new GuiPresentation();
            subclassChoicesGuiPresentation.Title = "Subclass/&SummonerSubclassPathTitle";
            subclassChoicesGuiPresentation.Description = "Subclass/&SummonerSubclassPathDescription";
            SummonerFeatureDefinitionSubclassChoice = this.BuildSubclassChoice(2, "Path", false, "SubclassChoiceSummonerSpecialistArchetypes", subclassChoicesGuiPresentation, SummonerClassSubclassesGuid);
        }

        // Rubikus
        static CharacterSubclassDefinition createPathOfTheRubikus()
        {
            //            createFacesOfFate();
//            createSummonRubikus();

            //            RuleDefinitions.DieType[] rubikus_dice = new RuleDefinitions.DieType[] { RuleDefinitions.DieType.D6, RuleDefinitions.DieType.D8, RuleDefinitions.DieType.D10, RuleDefinitions.DieType.D12 };

            var gui_presentation = new GuiPresentationBuilder(
                    "Subclass/&SummonerSubclassRubikusPathDescription",
                    "Subclass/&SummonerSubclassRubikusPathTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.OathOfTirmar.GuiPresentation.SpriteReference)
                    .Build();

            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder("SummonerSubclassRubikus", "b931f6f0-2d62-450c-a8d1-5379473d1234")
                    .SetGuiPresentation(gui_presentation)
//                    .AddFeatureAtLevel(summon_rubikus, 2)
                    .AddToDB();
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D6], 2)
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D8], 6)
            //                    .AddFeatureAtLevel(rubikus_fate_manipulation, 6)
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D10], 10)
            //                    .AddFeatureAtLevel(rubikus_concentrate_fate, 10)
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D12], 14)
            //                    .AddFeatureAtLevel(rubikus_fate_theft, 14)

            return definition;
        }

        static void createSummonRubikus()
        {
            string summon_rubikus_title_string = "Feature/&SummonerClassSummonRubikusPowerTitle";
            string summon_rubikus_description_string = "Feature/&SummonerClassSummonerRubikusPowerDescription";

            var summon_rubikus_image = SolastaModHelpers.CustomIcons.Tools.storeCustomIcon("SummonRubikusImage",
                                                                                           $@"{UnityModManagerNet.UnityModManager.modsPath}/SummonerClass/Sprites/SummonRubikus.png",
                                                                                           96, 99);

            var effectForm = new EffectForm();
            effectForm.FormType = EffectForm.EffectFormType.Summon;
            effectForm.SetCreatedByCharacter(true);

            SummonForm summonForm = new SummonForm();
            effectForm.SetSummonForm(summonForm);

            MonsterDefinition monsterDefinition = new MonsterDefinition();
            monsterDefinition = RubikusMonsterBuilder.AddToMonsterList();

            effectForm.SummonForm.SetMonsterDefinitionName(monsterDefinition.Name);
            effectForm.SummonForm.SetDecisionPackage(null);

            //Add to our new effect
            var effectDescription = new EffectDescription();
            effectDescription.Copy(DatabaseHelper.SpellDefinitions.ConjureAnimalsOneBeast.EffectDescription);
            effectDescription.DurationType = RuleDefinitions.DurationType.UntilLongRest;
            effectDescription.SetTargetSide(RuleDefinitions.Side.Ally);
            effectDescription.SetRangeParameter(24);
            effectDescription.EffectForms.Clear();
            effectDescription.EffectForms.Add(effectForm);

            summon_rubikus = Helpers.PowerBuilder.createPower("SummonerClassSummonRubikusPower",
                                                            "",
                                                            summon_rubikus_title_string,
                                                            summon_rubikus_description_string,
                                                            summon_rubikus_image,
                                                            DatabaseHelper.FeatureDefinitionPowers.PowerPaladinAuraOfProtection,
                                                            effectDescription,
                                                            RuleDefinitions.ActivationTime.Action,
                                                            1,
                                                            RuleDefinitions.UsesDetermination.Fixed,
                                                            RuleDefinitions.RechargeRate.AtWill,
                                                            Helpers.Stats.Charisma,
                                                            Helpers.Stats.Charisma
                                                            );
        }

        /*        static void createFacesOfFate()
                {
                    string faces_of_fate_title_string = "Feature/&SummonerSubclassFacesOfFateTitle";
                    string faces_of_fate_description_string = "Feature/&SummonerSubclassFacesOfFateDescription";

                    FeatureDefinitionPower previous_power = null;
                    var dice = inspiration_dice;
                    for (int i = 0; i < dice.Length; i++)
                    {
                        var inspiration_saves = Helpers.SavingThrowAffinityBuilder.createSavingthrowAffinity("BardClassInspirationSavingthrowBonus" + dice[i].ToString(),
                                                                                                                "",
                                                                                                                "",
                                                                                                                "",
                                                                                                                null,
                                                                                                                RuleDefinitions.CharacterSavingThrowAffinity.None,
                                                                                                                1,
                                                                                                                dice[i],
                                                                                                                Helpers.Stats.getAllStats().ToArray()
                                                                                                                );

                        var inspiration_skills = Helpers.AbilityCheckAffinityBuilder.createAbilityCheckAffinity("BardClassInspirationSkillsBonus" + dice[i].ToString(),
                                                                                                                    "",
                                                                                                                    "",
                                                                                                                    "",
                                                                                                                    null,
                                                                                                                    RuleDefinitions.CharacterAbilityCheckAffinity.None,
                                                                                                                    1,
                                                                                                                    dice[i],
                                                                                                                    Helpers.Stats.getAllStats().ToArray()
                                                                                                                    );

                        var inspiration_attack = Helpers.AttackBonusBuilder.createAttackBonus("BardClassInspirationAttackBonus" + dice[i].ToString(),
                                                                                                                    "",
                                                                                                                    "",
                                                                                                                    "",
                                                                                                                    null,
                                                                                                                    1,
                                                                                                                    dice[i]
                                                                                                                    );
                        var inspiration_condition = Helpers.ConditionBuilder.createConditionWithInterruptions("BardClassInspirationCondition" + dice[i].ToString(),
                                                                                                                "",
                                                                                                                Helpers.StringProcessing.concatenateStrings(Common.common_condition_prefix,
                                                                                                                                                            inspiration_title_string,
                                                                                                                                                            "Rules/&BardClassInspirationCondition" + dice[i].ToString()
                                                                                                                                                            ),
                                                                                                                inspiration_description_string,
                                                                                                                null,
                                                                                                                DatabaseHelper.ConditionDefinitions.ConditionGuided,
                                                                                                                new RuleDefinitions.ConditionInterruption[] {RuleDefinitions.ConditionInterruption.AbilityCheck,
                                                                                                                                                    RuleDefinitions.ConditionInterruption.Attacks,
                                                                                                                                                    RuleDefinitions.ConditionInterruption.SavingThrow },
                                                                                                                inspiration_saves,
                                                                                                                inspiration_skills,
                                                                                                                inspiration_attack
                                                                                                                );

                        var effect = new EffectDescription();
                        effect.Copy(DatabaseHelper.SpellDefinitions.Guidance.EffectDescription);
                        effect.SetRangeType(RuleDefinitions.RangeType.Distance);
                        effect.SetRangeParameter(12);
                        effect.DurationParameter = 10;
                        effect.DurationType = RuleDefinitions.DurationType.Minute;
                        effect.EffectForms.Clear();
                        effect.SetTargetFilteringTag((RuleDefinitions.TargetFilteringTag)ExtendedEnums.ExtraTargetFilteringTag.NonCaster);

                        var effect_form = new EffectForm();
                        effect_form.ConditionForm = new ConditionForm();
                        effect_form.FormType = EffectForm.EffectFormType.Condition;
                        effect_form.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
                        effect_form.ConditionForm.ConditionDefinition = inspiration_condition;
                        effect.EffectForms.Add(effect_form);

                        var inspiration_power = Helpers.PowerBuilder.createPower("BardInspirationPower" + dice[i].ToString(),
                                                                                    "",
                                                                                    Helpers.StringProcessing.appendToString(inspiration_title_string,
                                                                                                                            inspiration_title_string + dice[i].ToString(),
                                                                                                                            $" ({dice[i].ToString().ToString().ToLower()})"),
                                                                                    inspiration_description_string,
                                                                                    DatabaseHelper.SpellDefinitions.Guidance.GuiPresentation.SpriteReference,
                                                                                    DatabaseHelper.FeatureDefinitionPowers.PowerPaladinLayOnHands,
                                                                                    effect,
                                                                                    RuleDefinitions.ActivationTime.BonusAction,
                                                                                    0,
                                                                                    RuleDefinitions.UsesDetermination.AbilityBonusPlusFixed,
                                                                                    previous_power == null ? RuleDefinitions.RechargeRate.LongRest : RuleDefinitions.RechargeRate.ShortRest,
                                                                                    Helpers.Stats.Charisma,
                                                                                    Helpers.Stats.Charisma
                                                                                    );
                        inspiration_power.SetShortTitleOverride(inspiration_title_string);

                        if (previous_power != null)
                        {
                            inspiration_power.SetOverriddenPower(previous_power);
                        }
                        previous_power = inspiration_power;
                        inspiration_powers.Add(dice[i], inspiration_power);
                    }

                    string font_of_inspiration_title_string = "Feature/&BardClassFontOfInspirationFeatureTitle";
                    string font_of_inspiration_description_string = "Feature/&BardClassFontOfInspirationFeatureDescription";
                    font_of_inspiration = Helpers.OnlyDescriptionFeatureBuilder.createOnlyDescriptionFeature("BardClassFontOfInspirationFeature",
                                                                                                                "",
                                                                                                                font_of_inspiration_title_string,
                                                                                                                font_of_inspiration_description_string);
                }
        */

        // Puppetmaster
        static CharacterSubclassDefinition createPathOfThePuppetmaster()
        {
            //            RuleDefinitions.DieType[] rubikus_dice = new RuleDefinitions.DieType[] { RuleDefinitions.DieType.D6, RuleDefinitions.DieType.D8, RuleDefinitions.DieType.D10, RuleDefinitions.DieType.D12 };

            var gui_presentation = new GuiPresentationBuilder(
                    "Subclass/&SummonerSubclassPuppetmasterPathDescription",
                    "Subclass/&SummonerSubclassPuppetmasterPathTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.OathOfTheMotherland.GuiPresentation.SpriteReference)
                    .Build();

            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder("SummonerSubclassPuppetmaster", "b931f6f0-2d62-450c-a8d1-5379473e1234")
                    .SetGuiPresentation(gui_presentation)
                    .AddToDB();
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D6], 2)
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D8], 6)
            //                    .AddFeatureAtLevel(rubikus_fate_manipulation, 6)
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D10], 10)
            //                    .AddFeatureAtLevel(rubikus_concentrate_fate, 10)
            //                    .AddFeatureAtLevel(faces_of_fate[RuleDefinitions.DieType.D12], 14)
            //                    .AddFeatureAtLevel(rubikus_fate_theft, 14)

            return definition;
        }

        public static void BuildAndAddClassToDB()
        {
            var SummonerClass = new SummonerClassBuilder(SummonerClassName, SummonerClassNameGuid).AddToDB();
            SummonerClass.FeatureUnlocks.Sort(delegate (FeatureUnlockByLevel a, FeatureUnlockByLevel b)
                {
                    return a.Level - b.Level;
                }
            );

            SummonerFeatureDefinitionSubclassChoice.Subclasses.Add(createPathOfTheRubikus().Name);
            SummonerFeatureDefinitionSubclassChoice.Subclasses.Add(createPathOfThePuppetmaster().Name);
        }

        private static FeatureDefinitionSubclassChoice SummonerFeatureDefinitionSubclassChoice;
    }
}