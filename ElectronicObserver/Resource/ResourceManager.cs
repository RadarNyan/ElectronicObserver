﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Resource
{


	public sealed class ResourceManager
	{


		#region Singleton

		private static readonly ResourceManager instance = new ResourceManager();

		public static ResourceManager Instance => instance;

		#endregion


		#region Properties

		public ImageList Icons { get; private set; }

		public ImageList Equipments { get; private set; }

		public Icon AppIcon { get; private set; }

		#endregion


		#region Constants

		public static string AssetFilePath => "Assets.zip";

		#endregion


		public enum IconContent
		{
			Nothing = -1,
			AppIcon,
			ResourceFuel,
			ResourceAmmo,
			ResourceSteel,
			ResourceBauxite,
			ConditionSparkle,
			ConditionNormal,
			ConditionLittleTired,
			ConditionTired,
			ConditionVeryTired,
			ItemInstantRepair,
			ItemInstantConstruction,
			ItemDevelopmentMaterial,
			ItemModdingMaterial,
			ItemFurnitureCoin,
			ItemBlueprint,
			ItemCatapult,
			ItemPresentBox,
            ItemActionReport,
			FormArsenal,
			FormBattle,
			FormCompass,
			FormDock,
			FormFleet,
			FormHeadQuarters,
			FormInformation,
			FormLog,
			FormMain,
			FormQuest,
			FormShipGroup,
			FormBrowser,
			FormAlbumShip,
			FormAlbumEquipment,
			FormConfiguration,
			FormEquipmentList,
			FormWindowCapture,
			FormDropRecord,
			FormDevelopmentRecord,
			FormConstructionRecord,
			FormResourceChart,
			FormBaseAirCorps,
			FormJson,
			FormAntiAirDefense,
			FormFleetImageGenerator,
			FleetNoShip,
			FleetDocking,
			FleetSortieDamaged,
			FleetSortie,
			FleetExpedition,
			FleetDamaged,
			FleetNotReplenished,
			FleetAnchorageRepairing,
			FleetReady,
			FleetCombined,
			HeadQuartersShip,
			HeadQuartersEquipment,
			BrowserScreenShot,
			BrowserZoom,
			BrowserZoomIn,
			BrowserZoomOut,
			BrowserUnmute,
			BrowserMute,
			BrowserRefresh,
			BrowserNavigate,
			BrowserOther,
			RarityBlack,
			RarityRed,
			RarityBlueC,
			RarityBlueB,
			RarityBlueA,
			RaritySilver,
			RarityGold,
			RarityHoloB,
			RarityHoloA,
			RarityCherry,
			ParameterHP,
			ParameterFirepower,
			ParameterTorpedo,
			ParameterAA,
			ParameterArmor,
			ParameterASW,
			ParameterEvasion,
			ParameterLOS,
			ParameterLuck,
			ParameterBomber,
			ParameterAccuracy,
			ParameterAircraft,
			ParameterSpeed,
			ParameterRange,
			ParameterAircraftCost,
			ParameterAircraftDistance,
			BattleFormationEnemyLineAhead,
			BattleFormationEnemyDoubleLine,
			BattleFormationEnemyDiamond,
			BattleFormationEnemyEchelon,
			BattleFormationEnemyLineAbreast,
			AircraftLevel0,
			AircraftLevel1,
			AircraftLevel2,
			AircraftLevel3,
			AircraftLevel4,
			AircraftLevel5,
			AircraftLevel6,
			AircraftLevel7,
			AircraftLevelTop0,
			AircraftLevelTop1,
			AircraftLevelTop2,
			AircraftLevelTop3,
			AircraftLevelTop4,
			AircraftLevelTop5,
			AircraftLevelTop6,
			AircraftLevelTop7,
		}

		public enum EquipmentContent
		{
			Nothing,            //0
			MainGunS,
			MainGunM,
			MainGunL,
			SecondaryGun,
			Torpedo,
			CarrierBasedFighter,
			CarrierBasedBomber,
			CarrierBasedTorpedo,
			CarrierBasedRecon,
			Seaplane,
			Radar,
			AAShell,
			APShell,
			DamageControl,
			AAGun,
			HighAngleGun,
			DepthCharge,
			Sonar,
			Engine,
			LandingCraft,
			Autogyro,
			ASPatrol,
			Bulge,
			Searchlight,
			DrumCanister,
			RepairFacility,
			Flare,
			CommandFacility,
			MaintenanceTeam,
			AADirector,
			RocketArtillery,
			PicketCrew,
			FlyingBoat,
			Ration,
			Supplies,
			AmphibiousVehicle,
			LandAttacker,
			Interceptor,
			JetFightingBomberKeiun,
			JetFightingBomberKikka,
			TransportMaterials,
			SubmarineEquipment,
			SeaplaneFighter,
			ArmyInterceptor,
			NightFighter,
			NightAttacker,
			LandASPatrol,
			Locked,
			Unknown,
		}


		private ResourceManager()
		{

			Icons = new ImageList
			{
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new Size(16, 16)
			};

			Equipments = new ImageList
			{
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new Size(16, 16)
			};

		}


		public bool Load()
		{

			try
			{

				LoadFromArchive(AssetFilePath);
				return true;

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, "リソースファイルの読み込みに失敗しました。");
				MessageBox.Show("リソースファイルの読み込みに失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

				FillWithBlankImage(Icons, Enum.GetValues(typeof(IconContent)).Length);
				FillWithBlankImage(Equipments, Enum.GetValues(typeof(EquipmentContent)).Length);

			}

			return false;
		}



		private void LoadFromArchive(string path)
		{


			using (var archive = new ZipArchive(File.OpenRead(path), ZipArchiveMode.Read))
			{

				const string mstpath = @"Assets/";


				AppIcon = LoadIconFromArchive(archive, mstpath + @"AppIcon.ico");

				// ------------------------ icons ------------------------

				LoadImageFromArchive(Icons, archive, mstpath + @"AppIcon_16.png", "AppIcon");

				LoadImageFromArchive(Icons, archive, mstpath + @"Resource/Fuel.png", "Resource_Fuel");
				LoadImageFromArchive(Icons, archive, mstpath + @"Resource/Ammo.png", "Resource_Ammo");
				LoadImageFromArchive(Icons, archive, mstpath + @"Resource/Steel.png", "Resource_Steel");
				LoadImageFromArchive(Icons, archive, mstpath + @"Resource/Bauxite.png", "Resource_Bauxite");

				LoadImageFromArchive(Icons, archive, mstpath + @"Condition/Sparkle.png", "Condition_Sparkle");
				LoadImageFromArchive(Icons, archive, mstpath + @"Condition/Normal.png", "Condition_Normal");
				LoadImageFromArchive(Icons, archive, mstpath + @"Condition/LittleTired.png", "Condition_LittleTired");
				LoadImageFromArchive(Icons, archive, mstpath + @"Condition/Tired.png", "Condition_Tired");
				LoadImageFromArchive(Icons, archive, mstpath + @"Condition/VeryTired.png", "Condition_VeryTired");

				LoadImageFromArchive(Icons, archive, mstpath + @"Item/InstantRepair.png", "Item_InstantRepair");
				LoadImageFromArchive(Icons, archive, mstpath + @"Item/InstantConstruction.png", "Item_InstantConstruction");
				LoadImageFromArchive(Icons, archive, mstpath + @"Item/DevelopmentMaterial.png", "Item_DevelopmentMaterial");
				LoadImageFromArchive(Icons, archive, mstpath + @"Item/ModdingMaterial.png", "Item_ModdingMaterial");
				LoadImageFromArchive(Icons, archive, mstpath + @"Item/FurnitureCoin.png", "Item_FurnitureCoin");
				LoadImageFromArchive(Icons, archive, mstpath + @"Item/Blueprint.png", "Item_Blueprint");
				LoadImageFromArchive(Icons, archive, mstpath + @"Item/Catapult.png", "Item_Catapult");
				LoadImageFromArchive(Icons, archive, mstpath + @"Item/PresentBox.png", "Item_PresentBox");
                LoadImageFromArchive(Icons, archive, mstpath + @"Item/ActionReport.png", "Item_ActionReport");

                LoadImageFromArchive(Icons, archive, mstpath + @"Form/Arsenal.png", "Form_Arsenal");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Battle.png", "Form_Battle");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Compass.png", "Form_Compass");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Dock.png", "Form_Dock");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Fleet.png", "Form_Fleet");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Headquarters.png", "Form_Headquarters");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Information.png", "Form_Information");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Log.png", "Form_Log");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Main.png", "Form_Main");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Quest.png", "Form_Quest");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/ShipGroup.png", "Form_ShipGroup");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Browser.png", "Form_Browser");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/AlbumShip.png", "Form_AlbumShip");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/AlbumEquipment.png", "Form_AlbumEquipment");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Configuration.png", "Form_Configuration");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/EquipmentList.png", "Form_EquipmentList");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/WindowCapture.png", "Form_WindowCapture");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/DropRecord.png", "Form_DropRecord");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/DevelopmentRecord.png", "Form_DevelopmentRecord");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/ConstructionRecord.png", "Form_ConstructionRecord");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/ResourceChart.png", "Form_ResourceChart");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/BaseAirCorps.png", "Form_BaseAirCorps");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/Json.png", "Form_Json");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/AntiAirDefense.png", "Form_AntiAirDefense");
				LoadImageFromArchive(Icons, archive, mstpath + @"Form/FleetImageGenerator.png", "Form_FleetImageGenerator");

				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/NoShip.png", "Fleet_NoShip");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/Docking.png", "Fleet_Docking");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/SortieDamaged.png", "Fleet_SortieDamaged");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/Sortie.png", "Fleet_Sortie");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/Expedition.png", "Fleet_Expedition");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/Damaged.png", "Fleet_Damaged");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/NotReplenished.png", "Fleet_NotReplenished");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/AnchorageRepairing.png", "Fleet_AnchorageRepairing");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/Ready.png", "Fleet_Ready");
				LoadImageFromArchive(Icons, archive, mstpath + @"Fleet/Combined.png", "Fleet_Combined");

				LoadImageFromArchive(Icons, archive, mstpath + @"Headquarters/Ship.png", "HeadQuarters_Ship");
				LoadImageFromArchive(Icons, archive, mstpath + @"Headquarters/Equipment.png", "HeadQuarters_Equipment");

				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/ScreenShot.png", "Browser_ScreenShot");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/Zoom.png", "Browser_Zoom");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/ZoomIn.png", "Browser_ZoomIn");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/ZoomOut.png", "Browser_ZoomOut");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/Unmute.png", "Browser_Unmute");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/Mute.png", "Browser_Mute");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/Refresh.png", "Browser_Refresh");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/Navigate.png", "Browser_Navigate");
				LoadImageFromArchive(Icons, archive, mstpath + @"Browser/Other.png", "Browser_Other");

				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/Black.png", "Rarity_Black");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/Red.png", "Rarity_Red");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/BlueC.png", "Rarity_BlueC");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/BlueB.png", "Rarity_BlueB");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/BlueA.png", "Rarity_BlueA");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/Silver.png", "Rarity_Silver");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/Gold.png", "Rarity_Gold");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/HoloB.png", "Rarity_HoloB");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/HoloA.png", "Rarity_HoloA");
				LoadImageFromArchive(Icons, archive, mstpath + @"Rarity/Cherry.png", "Rarity_Cherry");

				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/HP.png", "Parameter_HP");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Firepower.png", "Parameter_Firepower");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Torpedo.png", "Parameter_Torpedo");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/AA.png", "Parameter_AA");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Armor.png", "Parameter_Armor");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/ASW.png", "Parameter_ASW");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Evasion.png", "Parameter_Evasion");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/LOS.png", "Parameter_LOS");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Luck.png", "Parameter_Luck");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Bomber.png", "Parameter_Bomber");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Accuracy.png", "Parameter_Accuracy");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Aircraft.png", "Parameter_Aircraft");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Speed.png", "Parameter_Speed");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/Range.png", "Parameter_Range");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/AircraftCost.png", "Parameter_AircraftCost");
				LoadImageFromArchive(Icons, archive, mstpath + @"Parameter/AircraftDistance.png", "Parameter_AircraftDistance");

				LoadImageFromArchive(Icons, archive, mstpath + @"Battle/FormationEnemy01.png", "Battle_FormationEnemy_LineAhead");
				LoadImageFromArchive(Icons, archive, mstpath + @"Battle/FormationEnemy02.png", "Battle_FormationEnemy_DoubleLine");
				LoadImageFromArchive(Icons, archive, mstpath + @"Battle/FormationEnemy03.png", "Battle_FormationEnemy_Diamond");
				LoadImageFromArchive(Icons, archive, mstpath + @"Battle/FormationEnemy04.png", "Battle_FormationEnemy_Echelon");
				LoadImageFromArchive(Icons, archive, mstpath + @"Battle/FormationEnemy05.png", "Battle_FormationEnemy_LineAbreast");

				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel0.png", "Level_AircraftLevel_0");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel1.png", "Level_AircraftLevel_1");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel2.png", "Level_AircraftLevel_2");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel3.png", "Level_AircraftLevel_3");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel4.png", "Level_AircraftLevel_4");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel5.png", "Level_AircraftLevel_5");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel6.png", "Level_AircraftLevel_6");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevel7.png", "Level_AircraftLevel_7");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop0.png", "Level_AircraftLevelTop_0");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop1.png", "Level_AircraftLevelTop_1");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop2.png", "Level_AircraftLevelTop_2");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop3.png", "Level_AircraftLevelTop_3");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop4.png", "Level_AircraftLevelTop_4");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop5.png", "Level_AircraftLevelTop_5");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop6.png", "Level_AircraftLevelTop_6");
				LoadImageFromArchive(Icons, archive, mstpath + @"Level/AircraftLevelTop7.png", "Level_AircraftLevelTop_7");


				// ------------------------ equipments ------------------------

				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Nothing.png", "Equipment_Nothing");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/MainGunS.png", "Equipment_MainGunS");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/MainGunM.png", "Equipment_MainGunM");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/MainGunL.png", "Equipment_MainGunL");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/SecondaryGun.png", "Equipment_SecondaryGun");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Torpedo.png", "Equipment_Torpedo");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/CarrierBasedFighter.png", "Equipment_CarrierBasedFighter");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/CarrierBasedBomber.png", "Equipment_CarrierBasedBomber");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/CarrierBasedTorpedo.png", "Equipment_CarrierBasedTorpedo");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/CarrierBasedRecon.png", "Equipment_CarrierBasedRecon");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Seaplane.png", "Equipment_Seaplane");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/RADAR.png", "Equipment_RADAR");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/AAShell.png", "Equipment_AAShell");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/APShell.png", "Equipment_APShell");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/DamageControl.png", "Equipment_DamageControl");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/AAGun.png", "Equipment_AAGun");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/HighAngleGun.png", "Equipment_HighAngleGun");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/DepthCharge.png", "Equipment_DepthCharge");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/SONAR.png", "Equipment_SONAR");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Engine.png", "Equipment_Engine");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/LandingCraft.png", "Equipment_LandingCraft");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Autogyro.png", "Equipment_Autogyro");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/ASPatrol.png", "Equipment_ASPatrol");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Bulge.png", "Equipment_Bulge");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Searchlight.png", "Equipment_Searchlight");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/DrumCanister.png", "Equipment_DrumCanister");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/RepairFacility.png", "Equipment_RepairFacility");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Flare.png", "Equipment_Flare");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/CommandFacility.png", "Equipment_CommandFacility");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/MaintenanceTeam.png", "Equipment_MaintenanceTeam");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/AADirector.png", "Equipment_AADirector");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/RocketArtillery.png", "Equipment_RocketArtillery");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/PicketCrew.png", "Equipment_PicketCrew");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/FlyingBoat.png", "Equipment_FlyingBoat");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Ration.png", "Equipment_Ration");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Supplies.png", "Equipment_Supplies");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/AmphibiousVehicle.png", "Equipment_AmphibiousVehicle");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/LandAttacker.png", "Equipment_LandAttacker");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Interceptor.png", "Equipment_Interceptor");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/JetFightingBomberKeiun.png", "Equipment_JetFightingBomberKeiun");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/JetFightingBomberKikka.png", "Equipment_JetFightingBomberKikka");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/TransportMaterials.png", "Equipment_TransportMaterials");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/SubmarineEquipment.png", "Equipment_SubmarineEquipment");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/SeaplaneFighter.png", "Equipment_SeaplaneFighter");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/ArmyInterceptor.png", "Equipment_ArmyInterceptor");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/NightFighter.png", "Equipment_NightFighter");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/NightAttacker.png", "Equipment_NightAttacker");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/LandASPatrol.png", "Equipment_LandASPatrol");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Locked.png", "Equipment_Locked");
				LoadImageFromArchive(Equipments, archive, mstpath + @"Equipment/Unknown.png", "Equipment_Unknown");

			}

		}

		private static void LoadImageFromArchive(ImageList imglist, ZipArchive arc, string path, string name)
		{

			var entry = arc.GetEntry(path);

			if (entry == null)
			{
				Utility.Logger.Add(3, string.Format("画像リソース {0} は存在しません。", path));
				imglist.Images.Add(name, new Bitmap(imglist.ImageSize.Width, imglist.ImageSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
				return;
			}


			try
			{

				Bitmap bmp = new Bitmap(entry.Open());

				if (bmp.Size != imglist.ImageSize)
				{

					bmp.Dispose();
					bmp = CreateBlankImage();

				}

				imglist.Images.Add(name, bmp);


			}
			catch (Exception)
			{

				Utility.Logger.Add(3, string.Format("画像リソース {0} の読み込みに失敗しました。", path));
				imglist.Images.Add(name, CreateBlankImage());
				return;
			}

		}

		private static Icon LoadIconFromArchive(ZipArchive arc, string path)
		{

			var entry = arc.GetEntry(path);

			if (entry == null)
			{
				Utility.Logger.Add(3, string.Format("画像リソース {0} は存在しません。", path));
				return null;
			}


			try
			{

				/*//ストリームから直接読み込むと不思議なチカラによってかき消される
				return new Icon( entry.Open() );
				/*/
				byte[] bytes;
				using (MemoryStream ms = new MemoryStream())
				{
					entry.Open().CopyTo(ms);
					bytes = ms.ToArray();
				}
				using (MemoryStream ms = new MemoryStream(bytes))
				{
					return new Icon(ms);
				}
				//*/

			}
			catch (Exception)
			{

				Utility.Logger.Add(3, string.Format("画像リソース {0} の読み込みに失敗しました。", path));
			}

			return null;
		}


		/// <summary>
		/// アーカイブの中からファイルをコピーします。
		/// </summary>
		/// <param name="archivePath">アーカイブの場所。</param>
		/// <param name="source">アーカイブ内のファイルのパス。</param>
		/// <param name="destination">出力するファイルのパス。</param>
		/// <param name="checkexist">true の場合、ファイルが既に存在するときコピーを中止します。</param>
		/// <param name="convertEncoding">エンコーディングを shift-jis から現在設定に合わせて変換するか。</param>
		/// <returns>コピーに成功すれば true 。それ以外は false 。</returns>
		public static bool CopyFromArchive(string archivePath, string source, string destination, bool checkexist = true, bool convertEncoding = false)
		{

			if (checkexist && File.Exists(destination))
			{
				return false;
			}


			using (var archive = new ZipArchive(File.OpenRead(archivePath), ZipArchiveMode.Read))
			{

				string entrypath = @"Assets/" + source;

				var entry = archive.GetEntry(entrypath);

				if (entry == null)
				{
					Utility.Logger.Add(3, string.Format("{0} は存在しません。", entrypath));
					return false;
				}


				try
				{

					if (convertEncoding && Utility.Configuration.Config.Log.FileEncodingID != 4)
					{
						using (var filetoconvert = GetStreamFromArchive(source))
						{
							filetoconvert.Position = 0;
							using (var convertStream = new StreamReader(filetoconvert, Encoding.GetEncoding(932)))
							{
								string fileread = convertStream.ReadToEnd();
								File.WriteAllText(destination, fileread, Utility.Configuration.Config.Log.FileEncoding);
							}
						}
					}
					else
					{
						entry.ExtractToFile(destination);
					}
					Utility.Logger.Add(2, string.Format("{0} をコピーしました。", entrypath));

				}
				catch (Exception ex)
				{

					Utility.Logger.Add(3, string.Format("{0} のコピーに失敗しました。{1}", entrypath, ex.Message));
					return false;
				}
			}

			return true;
		}


		/// <summary>
		/// アーカイブの中からファイルをコピーします。
		/// </summary>
		/// <param name="source">アーカイブ内のファイルのパス。</param>
		/// <param name="destination">出力するファイルのパス。</param>
		/// <param name="checkexist">true の場合、ファイルが既に存在するときコピーを中止します。</param>
		/// <param name="convertEncoding">エンコーディングを shift-jis から現在設定に合わせて変換するか。</param>
		/// <returns>コピーに成功すれば true 。それ以外は false 。</returns>
		public static bool CopyFromArchive(string source, string destination, bool checkexist = true, bool convertEncoding = false)
		{
			return CopyFromArchive(AssetFilePath, source, destination, checkexist);
		}


		/// <summary>
		/// アーカイブの中から文書ファイルをコピーします。エンコーディングは現在設定に合わせて自動で変更されます。
		/// </summary>
		/// <param name="source">アーカイブ内のファイルのパス。</param>
		/// <param name="destination">出力するファイルのパス。</param>
		/// <returns>コピーに成功すれば true 。それ以外は false 。</returns>
		public static bool CopyDocumentFromArchive(string source, string destination)
		{
			return CopyFromArchive(AssetFilePath, source, destination, true, true);
		}


		/// <summary>
		/// アーカイブからファイルを選択し、ストリームを開きます。
		/// </summary>
		/// <param name="archivePath">アーカイブの場所。</param>
		/// <param name="source">アーカイブ内のファイルのパス。</param>
		/// <returns>ファイルのストリーム。オープンに失敗した場合は null を返します。</returns>
		public static MemoryStream GetStreamFromArchive(string archivePath, string source)
		{
			using (var archive = new ZipArchive(File.OpenRead(archivePath), ZipArchiveMode.Read))
			{

				string entrypath = @"Assets/" + source;

				var entry = archive.GetEntry(entrypath);

				if (entry == null)
				{
					Utility.Logger.Add(3, string.Format("{0} は存在しません。", entrypath));
					return null;
				}


				try
				{

					byte[] bytes;
					using (MemoryStream ms = new MemoryStream())
					{
						var st = entry.Open();
						st.CopyTo(ms);
						bytes = ms.ToArray();
						st.Close();
					}

					return new MemoryStream(bytes);

				}
				catch (Exception ex)
				{

					Utility.Logger.Add(3, string.Format("{0} の展開に失敗しました。{1}", entrypath, ex.Message));
					return null;
				}
			}

		}


		/// <summary>
		/// アーカイブからファイルを選択し、ストリームを開きます。
		/// </summary>
		/// <param name="source">アーカイブ内のファイルのパス。</param>
		/// <returns>ファイルのストリーム。オープンに失敗した場合は null を返します。</returns>
		public static MemoryStream GetStreamFromArchive(string source)
		{
			return GetStreamFromArchive(AssetFilePath, source);
		}


		/// <summary>
		/// エラーが発生しないよう、ダミーの画像で領域を埋めます。
		/// </summary>
		private static void FillWithBlankImage(ImageList list, int length)
		{

			for (int i = list.Images.Count; i < length; i++)
			{
				list.Images.Add(CreateBlankImage());
			}
		}

		/// <summary>
		/// 空白画像を作成します。
		/// </summary>
		private static Bitmap CreateBlankImage()
		{
			return new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		}



		/// <summary>
		/// 装備アイコンを取得します。一般的用途(ロック/未装備等でない、マスターとしてのアイコン)に向いています。
		/// </summary>
		public static Image GetEquipmentImage(int imageID)
		{

			if (imageID < 0)
				return Instance.Equipments.Images[(int)EquipmentContent.Locked];
			if (imageID >= (int)EquipmentContent.Locked)
				return Instance.Equipments.Images[(int)EquipmentContent.Unknown];

			return Instance.Equipments.Images[imageID];
		}



		/// <summary>
		/// BitmapをIconに変換します。
		/// </summary>
		public static Icon BitmapToIcon(Bitmap image)
		{
			return Icon.FromHandle(image.GetHicon());
		}

		/// <summary>
		/// ImageをIconに変換します。
		/// </summary>
		public static Icon ImageToIcon(Image image)
		{
			return BitmapToIcon((Bitmap)image);
		}


		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		public extern static bool DestroyIcon(IntPtr handle);

		/// <summary>
		/// ImageToIcon を利用して生成したアイコンを破棄する場合、必ずこのメソッドを呼んで破棄してください。
		/// </summary>
		public static bool DestroyIcon(Icon icon)
		{
			return DestroyIcon(icon.Handle);
		}

	}


}
