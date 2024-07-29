using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;
using LudeonTK;
using Verse.Noise;
using Verse.AI;
using System.Security.Cryptography.X509Certificates;
using HarmonyLib;
using Unity.Jobs;
using static UnityEngine.GraphicsBuffer;
using RimWorld.BaseGen;
using static System.Net.Mime.MediaTypeNames;
using Verse.AI.Group;
using System.Security.Cryptography;
using System.Data.SqlTypes;
using System.Net.NetworkInformation;
using RimWorld.QuestGen;
using System.Xml.Linq;

namespace WN
{
    [DefOf]
    public static class WN_DefOf
    {
        public static ThingDef WN_Ring;
        public static ThingDef ApGuardianWeapon;
        public static ThingDef ApScytheWeapon;
        public static ThingDef ApSpearWeapon;
        public static ThingDef ApStaffWeapon;
        public static HediffDef Aleph;
        public static HediffDef PaleDamage;
        public static HediffDef QliphothWN;
        public static EffecterDef RingEffect;
        public static HediffDef AbnormalityResurrection;
        public static EffecterDef ApScytheEffect;
        public static EffecterDef ApSpearEffect;
        public static EffecterDef ApWarnEffect;
        public static DamageDef ApScytheCut;
        public static DamageDef ApSpearCut;
        public static EffecterDef ApDashEffect;
        public static FleckDef Fleck_ApSlash;
        public static FleckDef HoraxianSparks;
        public static FleckDef Fleck_PDLEffect;
        public static ThingDef ApFlyer;
        public static DamageDef ApStaffDamage;
        public static DamageDef BlackSubDamage;
        public static HediffDef BlackDamage;
        public static HediffDef WhiteDamage;
        public static HediffDef WNegoGift;
        public static MentalStateDef MSSuicide;
        public static HediffDef PlagueDoctorBlessing;
        public static HediffDef BrokenPDBlessing;
        public static HediffDef Zayin;
        public static HediffDef WN_whatthesigma;
        public static PawnKindDef FirstApostle;
        public static PawnKindDef SecondApostle;
        public static PawnKindDef ThirdApostle;
        public static PawnKindDef FourthApostle;
        public static PawnKindDef FifthApostle;
        public static PawnKindDef SixthApostle;
        public static PawnKindDef SeventhApostle;
        public static PawnKindDef EighthApostle;
        public static PawnKindDef NinthApostle;
        public static PawnKindDef TenthApostle;
        public static PawnKindDef EleventhApostle;
        public static PawnKindDef WhiteNight;
        public static PawnKindDef PlagueDoctor;
        public static ThingDef ParadiseLostWeapon;
        public static EntityCodexEntryDef WN_Entry;
        public static EntityCodexEntryDef PD_Entry;
        public static ThingDef PDLArmor;
        static WN_DefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(WN_DefOf));
    }

    
    /*
    public class JobGiver_ApostleScytheAttack : ThinkNode_JobGiver
    {
        private float maxAttackDistance = 56f;
        protected override Job TryGiveJob(Pawn pawn)
        {
            Thing attackTarget = this.FindAttackTarget(pawn);
            Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, (LocalTargetInfo)attackTarget);
            job.canUseRangedWeapon = false;
            return job;
        }

        private Thing FindAttackTarget(Pawn pawn)
        {
            return (Thing)AttackTargetFinder.BestAttackTarget((IAttackTargetSearcher)pawn, TargetScanFlags.NeedReachable, new Predicate<Thing>(this.IsGoodTarget), maxDist: this.maxAttackDistance, canBashDoors: true);
        }

        protected virtual bool IsGoodTarget(Thing thing)
        {
            return (thing is Pawn pawn && pawn.Spawned && !pawn.Downed && !pawn.IsPsychologicallyInvisible()) || thing is Building_Turret;
        }
    }
    
    public class JobGiver_ApostleGuardianAttack : ThinkNode_JobGiver
    {
        private float maxAttackDistance = 40f;
        private float guardDistance = 30f;
        protected override Job TryGiveJob(Pawn pawn)
        {
            Thing attackTarget = this.FindAttackTarget(pawn);
            Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, (LocalTargetInfo)attackTarget);
            job.canUseRangedWeapon = false;
            return job;
        }

        private Thing FindAttackTarget(Pawn pawn)
        {
            IntVec3 Locus = default;
            Pawn wn = null;
            if (pawn.HasComp<CompApostleGuardian>())
            {
                wn = pawn.GetComp<CompApostleGuardian>().WhiteNight;
            }
            if (wn != null)
            {
                Locus = wn.Position;
            }
            return (Thing)AttackTargetFinder.BestAttackTarget((IAttackTargetSearcher)pawn, TargetScanFlags.NeedReachable, new Predicate<Thing>(this.IsGoodTarget), maxTravelRadiusFromLocus: guardDistance, locus: Locus, maxDist: this.maxAttackDistance, canBashDoors: true);
        }

        protected virtual bool IsGoodTarget(Thing thing)
        {
            return (thing is Pawn pawn && pawn.Spawned && !pawn.Downed && !pawn.IsPsychologicallyInvisible()) || thing is Building_Turret;
        }
    }
    */
    public class CompProperties_ApostleGuardian : CompProperties
    {
        public CompProperties_ApostleGuardian() => this.compClass = typeof(CompApostleGuardian);
    }

    public class CompProperties_ApostleScythe : CompProperties
    {
        public CompProperties_ApostleScythe() => this.compClass = typeof(CompApostleScythe);
    }

    public class CompProperties_ApostleSpear : CompProperties
    {
        public CompProperties_ApostleSpear() => this.compClass = typeof(CompApostleSpear);
    }
    public class IncidentWorker_PlagueDoctorArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            
            Map target = (Map)parms.target;
            PawnKindDef pd = WN_DefOf.PlagueDoctor;
            IntVec3 result;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out result, target, CellFinder.EdgeRoadChance_Animal))
                return false;
            IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, target, 10);
            Thing trg = GenSpawn.Spawn((Thing)PawnGenerator.GeneratePawn(pd), loc, target);
            this.SendStandardLetter(this.def.letterLabel,this.def.letterText,this.def.letterDef,parms,lookTargets:(LookTargets)trg);
            return true;
        }

        
    }
    public class LobCoUtility
    {
        private static float ArmorDamageMult(Pawn pawn,float damage,int type)
        {
            if (pawn.apparel == null)
            {
                return damage;
            }
            foreach (Thing item in pawn.apparel.WornApparel)
            {
                if (item.HasComp<LobCoArmor>())
                {
                    if (type == 0)
                    {
                        damage = item.TryGetComp<LobCoArmor>().CutWhiteDamage(damage);
                    }
                    else if (type == 1)
                    {
                        damage = item.TryGetComp<LobCoArmor>().CutBlackDamage(damage);
                    }
                    else if (type == 2)
                    {
                        damage = item.TryGetComp<LobCoArmor>().CutPaleDamage(damage);
                    }
                }
            }
            return damage;
        }
        public static void DealPaleDamage(Pawn pawn, float damage, Pawn caster)
        {
            if (pawn == null)
            {
                return;
            }
            if (caster != null && pawn.Faction != null)
            {
                pawn.Faction.Notify_MemberTookDamage(pawn, new DamageInfo(DamageDefOf.Deterioration, damage, 0f, -1, caster));
            }
            damage = ArmorDamageMult(pawn,damage,2);
            if (pawn is LobCoPawn lcp)
            {
                lcp.TakePaleDamage(damage);
            }
            else
            {
                Hediff hd = pawn.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.PaleDamage);
                if (hd == null)
                {
                    hd = pawn.health.AddHediff(WN_DefOf.PaleDamage);
                }
                hd.Severity += damage / 100f;
            }
        }

        public static void DealBlackDamage(Pawn pawn, float damage,Pawn caster)
        {
            if (pawn == null)
            {
                return;
            }
            if (caster != null && pawn.Faction != null)
            {
                pawn.Faction.Notify_MemberTookDamage(pawn, new DamageInfo(DamageDefOf.Deterioration, damage, 0f, -1, caster));
            }
            damage = ArmorDamageMult(pawn, damage, 1);
            if (pawn is LobCoPawn lcp)
            {
                lcp.TakeBlackDamage(damage);
            }
            else
            {
                Hediff hd = pawn.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.BlackDamage);
                if (hd == null)
                {
                    hd = pawn.health.AddHediff(WN_DefOf.BlackDamage);
                }
                hd.Severity += damage / 100f;
            }
        }

        public static void DealWhiteDamage(Pawn pawn, float damage, Pawn caster)
        {
            if (pawn == null)
            {
                return;
            }
            if (caster != null && pawn.Faction != null)
            {
                pawn.Faction.Notify_MemberTookDamage(pawn, new DamageInfo(DamageDefOf.Deterioration, damage, 0f, -1, caster));
            }
            damage = ArmorDamageMult(pawn, damage, 0);
            if (pawn is LobCoPawn lcp)
            {
                lcp.TakeWhiteDamage(damage);
            }
            else
            {
                Hediff hd = pawn.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.WhiteDamage);
                if (hd == null)
                {
                    hd = pawn.health.AddHediff(WN_DefOf.WhiteDamage);
                }
                hd.Severity += damage / 100f;
            }
        }
    }
    public class CompProperties_ApostleStaff : CompProperties
    {
        public CompProperties_ApostleStaff() => this.compClass = typeof(CompApostleStaff);
    }
    public class CompProperties_PDLArmor : CompProperties
    {
        public CompProperties_PDLArmor() => this.compClass = typeof(CompPDLArmor);
    }
    public class CompProperties_PlagueDoctor : CompProperties
    {
        public CompProperties_PlagueDoctor() => this.compClass = typeof(CompPlagueDoctor);
    }

    public class Verb_ParadiseLostMain : Verb
    {
        private float radius = 5.9f;
        private float damage = 25f;
        protected override bool TryCastShot()
        {
            ShootLine resultingLine;
            bool shootLineFromTo = this.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out resultingLine);
            if ((this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map) || !shootLineFromTo)
                return false;
            return true;
        }
        public override void WarmupComplete()
        {
            base.WarmupComplete();
            int numCells = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < numCells; ++i)
            {
                IntVec3 c = GenRadial.RadialPattern[i];

                c = c + currentTarget.Cell;
                if (c.InBounds(Caster.Map))
                {

                    List<Thing> thingList = c.GetThingList(Caster.Map);
                    for (int j = 0; j < thingList.Count; ++j)
                    {
                        Thing thing = thingList[j];
                        if (thing is Pawn pawn)
                        {
                            OnHit(pawn);
                        }

                    }
                }
            }
            foreach (Thing item in (caster as Pawn).apparel.WornApparel)
            {
                if (item.HasComp<CompPDLArmor>())
                {
                    if (Rand.Chance(0.33f))
                        item.TryGetComp<CompPDLArmor>().shield = 100f;
                    foreach (Hediff hediff in (caster as Pawn).health.hediffSet.hediffs)
                    {
                        if (hediff.def.isBad && hediff.def.tendable && !hediff.def.isInfection && !hediff.def.chronic)
                        {
                            hediff.Severity -= 4f;
                        }
                    }

                }
            }

        }

        private void OnHit(Pawn pawn)
        {
            if (pawn == null )
            {
                return;
            }
            if (pawn.Faction != Caster.Faction)
            {
                FleckMaker.Static(pawn.Position.ToVector3Shifted(),pawn.Map, WN_DefOf.Fleck_PDLEffect, 2f);
                int m = UnityEngine.Random.Range(3, 5);
                Map mp = pawn.Map;
                if (mp == null)
                {
                    mp = this.Caster.Map;
                }
                if (this.Caster.Map == null)
                {
                    return;
                }
                LobCoUtility.DealPaleDamage(pawn, damage * 0.6f * UnityEngine.Random.Range(0.85f, 1.2f),(Pawn)this.caster);
                WN_DefOf.ApSpearEffect.Spawn().Trigger(new TargetInfo(this.Caster.Position, this.Caster.Map), new TargetInfo(pawn.Position, mp));
                pawn.stances.stagger.StaggerFor(200, 0.3f);
                DamageInfo dinfo = new DamageInfo();            
                dinfo.SetAllowDamagePropagation(false);
                dinfo.intendedTargetInt = pawn;                             
                for (int i = 0;i < m;i++)
                {
                    int d = UnityEngine.Random.Range(1, 3);
                    if (d == 1)
                    {
                        dinfo.Def = DamageDefOf.Cut;
                    }
                    else
                    {
                        dinfo.Def = DamageDefOf.Stab;
                    }                    
                    dinfo.SetAmount(damage * 0.6f * UnityEngine.Random.Range(0.7f, 1.4f));
                    
                    AccessTools.StructFieldRefAccess<DamageInfo, float>(ref dinfo, "armorPenetrationInt") = 0.6f * UnityEngine.Random.Range(0.7f, 1.2f);
                    pawn.TakeDamage(dinfo);
                }
            }
        }
        public override void DrawHighlight(LocalTargetInfo target)
        {
            base.DrawHighlight(target);
            
            List<IntVec3> list = new List<IntVec3>();

            int numCells = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < numCells; ++i)
            {
                IntVec3 c = GenRadial.RadialPattern[i];

                c = c + target.Cell;
                if (c.InBounds(Caster.Map))
                {
                    list.Add(c);
                }
            }
            Color color = Color.white;
            float? altOffset = new float?();
            if (list.Count > 0)
            {
                GenDraw.DrawFieldEdges(list, color, altOffset);
            }
        }

    }
    public class PlagueDoctor : Pawn
    {
        public List<Pawn> Apostles;
        public bool advented = false;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (this.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.Zayin) == null)
            {
                this.health.AddHediff(WN_DefOf.Zayin);
            }
            if (Find.EntityCodex == null)
            {
                return;
            }
            if (!Find.EntityCodex.Discovered(WN_DefOf.PD_Entry))
            {
                Find.EntityCodex.SetDiscovered(WN_DefOf.PD_Entry);
            }
            HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(this);
            HealthUtility.FixWorstHealthCondition(this);
            HealthUtility.FixWorstHealthCondition(this);
            HealthUtility.FixWorstHealthCondition(this);
            HealthUtility.FixWorstHealthCondition(this);
        }
        public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
        {       
            if (studier.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.PlagueDoctorBlessing) == null && studier.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.BrokenPDBlessing) == null)
            {
                Hediff hediff = studier.health.AddHediff(WN_DefOf.PlagueDoctorBlessing);
                if (Apostles == null)
                {
                    Apostles = new List<Pawn>();
                }
                if (hediff.TryGetComp<CompBlessing>() != null) 
                {
                    hediff.TryGetComp<CompBlessing>().Doctor = this;
                    hediff.TryGetComp<CompBlessing>().Order = Apostles.Count;
                    //Log.Message(studier.Name + " is apostle " + (Apostles.Count + 1).ToString());
                } 
                if (!Apostles.Contains(studier))
                {
                    Apostles.Add(studier);
                }

                HealthUtility.FixWorstHealthCondition(studier);
                HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(studier);
                
            }
            if (Apostles.Count == 12 && !advented)
            {
                advented = true;
                WN.WhiteNight.Advent(this, null);
            }
            else
            {
                base.Notify_Studied(studier, amount, category);
            }

            
            
        }

        
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<Pawn>(ref Apostles, "Apostles", false,LookMode.Reference);
            Scribe_Values.Look(ref advented,"advented");
        }
    }

    public class CompPlagueDoctor : ThingComp
    {
        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            UnBless();
            base.PostDestroy(mode, previousMap);
        }
        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            UnBless();
            base.Notify_Killed(prevMap, dinfo);          
        }

        private void UnBless()
        {
            PlagueDoctor pd = this.parent as PlagueDoctor;
            if (pd == null)
            {
                return;
            }
            int a = 0;
            if (pd.Apostles == null)
            {
                return;
            }
            foreach (Pawn p in pd.Apostles)
            {
                if (p != null && a != 11)
                {
                    Hediff blessing = p.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.PlagueDoctorBlessing);
                    if (blessing != null)
                    {
                        p.health.RemoveHediff(blessing);
                    }
                    Hediff unblessing = p.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.BrokenPDBlessing);
                    if (unblessing == null)
                    {
                        p.health.AddHediff(WN_DefOf.BrokenPDBlessing);
                    }
                }
                a++;
            }
        }
    }
    public class CompApostleStaff : CompApostle
    {
        protected int counter = 6;
        protected int staffcooldown = 28;
        protected int attackcounter = -1;
        protected float staffrange = 28f;
        protected int delay = 3;
        protected float damage = 40f;
        protected float width = 1.2f;
        protected IntVec3 target = default;
        private Map map;
        private float minrange = 7f;
        private int duration = 10;
        private int durationleft = 0;

        private bool shooting = false;


        public override void CompTick()
        {
            if (map == null)
            {
                map = this.parent.Map;
            }
            if (map == null)
                return;
            if (Gen.IsHashIntervalTick(this.parent, 30))
            {
                if (WhiteNight == null)
                {
                    counter--;
                }
                else
                {
                    counter = 3;
                }
                if (counter <= 0)
                {
                    //Die();
                }
                if (!(this.parent as Pawn).Downed)
                {
                    if (staffcooldown <= 0 && shooting == false)
                    {
                        StaffAttackStart();
                    }
                    staffcooldown--;
                    attackcounter--;
                    if (attackcounter <= 0 && shooting == true)
                    {
                        StaffAttack(target);
                        durationleft--;
                        if (durationleft <= 0)
                        {
                            shooting = false;
                        }
                    }
                }
            }

        }

        private void StaffAttackStart()
        {

            bool success = false;
            if (this.parent is Pawn pawn)
            {
                int numCells = GenRadial.NumCellsInRadius(staffrange);
                for (int i = numCells - 1; i >= 0; i--)
                {
                    IntVec3 c = GenRadial.RadialPattern[i];

                    c = c + pawn.Position;
                    if (c.InBounds(pawn.Map))
                    {
                        List<Thing> thingList = c.GetThingList(pawn.Map);
                        for (int j = 0; j < thingList.Count; ++j)
                        {
                            Thing thing = thingList[j];
                            if (!success && thing is Pawn t && IsValidtarget(t) && (t.Position - pawn.Position).LengthHorizontalSquared >= minrange * minrange)
                            {
                               
                                target = t.Position;
                                success = true;
                                staffcooldown = 28;
                                shooting = true;
                                durationleft = duration;
                                attackcounter = delay;
                                pawn.stances.stagger.StaggerFor(delay * 3000, 0f);
                                WN_DefOf.ApWarnEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                                map = t.Map;
                            }

                        }
                    }
                }
            }
        }

        private bool IsValidtarget(Pawn pawn)
        {
            if (pawn == null || pawn.health.Dead || pawn is Apostle || pawn.HasComp<CompWhiteNight>() || !GenSight.LineOfSightToEdges(this.parent.Position,pawn.Position,this.parent.Map))
                return false;
            return true;
        }

        private void StaffAttack(IntVec3 target)
        {
            if (map == null)
            {
                map = this.parent.Map;
            }
            
            (this.parent as Pawn).stances.stagger.StaggerFor(3000, 0f);
            List<IntVec3> list = new List<IntVec3>();
            
            List<Pawn> hitThings = new List<Pawn>();
            
            Pawn pawn = (Pawn)this.parent;
            List<IntVec3> cells = (List<IntVec3>)GenSight.BresenhamCellsBetween(this.parent.Position, target);
            


            foreach (IntVec3 cell in cells)
            {
                List<Thing> thingList = map.thingGrid.ThingsListAtFast(cell);
                foreach (Thing thing in thingList)
                {
                    if (thing.def.IsDoor && !(thing as Building_Door).Open || thing.def.passability == Traversability.Impassable || thing is Pawn && (double)(thing as Pawn).BodySize > (double)pawn.BodySize * 1.1f)
                    {

                        list.Add(thing.Position);

                    }

                }
            }



            IntVec3 closestcell = target;
            bool flag = false;
            foreach (IntVec3 c in list)
            {
                if ((c - pawn.Position).LengthHorizontalSquared <= (closestcell - pawn.Position).LengthHorizontal)
                {
                    closestcell = c;
                    flag = true;
                }
            }
            if (!flag)
            {
                closestcell = closestcell + new IntVec3((closestcell - pawn.Position).ToVector3() * 0.15f);
            }
            cells = (List<IntVec3>)GenSight.BresenhamCellsBetween(this.parent.Position, closestcell);
            int ccount = cells.Count;
            for (int i = 0; i < ccount; i++)
            {
                float scale = (cells[i] - this.parent.Position).LengthHorizontal / 20f;
                for (int ii = 0;ii < 5;ii++)
                {
                    FleckMaker.Static(cells[i].ToVector3Shifted() + new Vector3(UnityEngine.Random.Range(-0.8f,0.8f),0f, UnityEngine.Random.Range(-0.8f, 0.8f)), map, WN_DefOf.HoraxianSparks, 1.8f + scale);
                }
                
                AddAdjacent(cells, cells[i]);
            }




            foreach (IntVec3 cell in cells)
            {
                if (!cell.InBounds(map))
                {
                    continue;
                }
                
                List<Thing> thingList = map.thingGrid.ThingsListAtFast(cell);
                foreach (Thing thing in thingList)
                {
                    if (thing is Pawn p)
                    {
                        hitThings.Add(p);
                    }

                }
            }

            foreach (Pawn p in hitThings)
            {
                OnHit(p);
            }
            


        }

        private void OnHit(Pawn pawn)
        {
            if (IsValidtarget(pawn))
            {

                LobCoUtility.DealWhiteDamage(pawn,damage,(Pawn)this.parent);
                WN_DefOf.ApSpearEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                if (pawn.stances != null && pawn.stances.stagger != null)
                {
                    pawn.stances.stagger.StaggerFor(30, 0.3f);
                }
                DamageInfo dinfo = new DamageInfo();
                dinfo.Def = WN_DefOf.ApStaffDamage;
                dinfo.SetAmount(damage*0.6f);
                dinfo.SetAllowDamagePropagation(true);
                dinfo.SetIgnoreArmor(true);
                dinfo.intendedTargetInt = pawn;
                AccessTools.StructFieldRefAccess<DamageInfo, float>(ref dinfo, "armorPenetrationInt") = 0.75f;
                pawn.TakeDamage(dinfo);
            }
        }

        private void Die()
        {
            this.parent.Kill();
        }

        private void AddAdjacent(List<IntVec3> things, IntVec3 cell)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    IntVec3 ncell = cell;
                    ncell.x += x;
                    ncell.z += y;
                    if (!things.Contains(ncell))
                    {
                        things.Add(ncell);
                    }
                }
            }
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref shooting, "shooting");
            Scribe_Values.Look<int>(ref durationleft, "durationleft");
            Scribe_Values.Look(ref attackcounter, "attackcounter");
            Scribe_Values.Look(ref staffcooldown, "staffcooldown");
            Scribe_Values.Look(ref target, "target");
        }
        
    }
    public class CompApostleGuardian : CompApostle
    {
        protected int counter = 6;
        protected int slashcooldown = 16;
        protected int attackcounter = -1;
        protected float slashrange = 5.9f;
        protected float slashanglecos = 0.5f;
        protected int delay = 4;
        protected float damage = 170f;
        protected IntVec3 target = default;
        protected IntVec3 btarget = default;

        public override void CompTick()
        {
            if (Gen.IsHashIntervalTick(this.parent, 30))
            {
                if (WhiteNight == null)
                {
                    counter--;
                }
                else
                {
                    counter = 3;
                }
                if (counter <= 0)
                {
                    //Die();
                }
                if (!(this.parent as Pawn).Downed)
                {
                    if (slashcooldown <= 0)
                    {
                        target = new IntVec3(-9999, -9999, -9999);
                        btarget = new IntVec3(-9999, -9999, -9999);
                        SlashAttackStart();
                    }
                    slashcooldown--;
                    attackcounter--;
                    if (attackcounter == 0)
                    {
                        if (target != new IntVec3(-9999, -9999, -9999))
                        {
                            SlashAttack(target);
                        }
                        else if (btarget != new IntVec3(-9999, -9999, -9999))
                        {
                            SlashAttack(btarget);
                        }
                    }
                }
            }
            
        }

        private void SlashAttackStart()
        {
            bool bsuccess = false;
            bool success = false;
            if (this.parent is Pawn pawn)
            {
                int numCells = GenRadial.NumCellsInRadius(slashrange - 2f);
                for (int i = 0; i < numCells; ++i)
                {
                    IntVec3 c = GenRadial.RadialPattern[i];

                    c = c + pawn.Position;
                    if (c.InBounds(pawn.Map))
                    {
                        List<Thing> thingList = c.GetThingList(pawn.Map);
                        for (int j = 0; j < thingList.Count; ++j)
                        {
                            Thing thing = thingList[j];
                            if(!success && thing is Pawn t && IsValidtarget(t))
                            {
                                target = t.Position;
                                success = true;
                                slashcooldown = 16;
                                attackcounter = delay;
                                pawn.stances.stagger.StaggerFor(delay * 3000,0f);
                                WN_DefOf.ApWarnEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                            }
                            if (!bsuccess && thing is Building b && b.Faction != null && b.Faction != Faction.OfAncients)
                            {
                                btarget = thing.Position;
                                bsuccess = true;
                                slashcooldown = 16;
                                attackcounter = delay;
                                pawn.stances.stagger.StaggerFor(delay * 3000, 0f);
                                WN_DefOf.ApWarnEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                            }

                        }
                    }
                }                                            
            }
        }

        private bool IsValidtarget(Pawn pawn)
        {
            if (pawn == null || pawn.health.Dead || pawn is Apostle || pawn.HasComp<CompWhiteNight>() || !GenSight.LineOfSightToEdges(this.parent.Position, pawn.Position, this.parent.Map))
                return false;
            return true;
        }

        private void SlashAttack(IntVec3 target)
        {
            
            int numCells = GenRadial.NumCellsInRadius(slashrange);
            for (int i = 0; i < numCells; ++i)
            {
                IntVec3 c = GenRadial.RadialPattern[i];

                c = c + this.parent.Position;
                if (c.InBounds(this.parent.Map) && GetAngleCos(target, c) >= slashanglecos)
                {
                    float scale = (c - this.parent.Position).LengthHorizontal/2f + 0.5f;
                    FleckMaker.Static(c, this.parent.Map, WN_DefOf.Fleck_ApSlash,scale);
                    List<Thing> thingList = c.GetThingList(this.parent.Map);
                    for (int j = 0; j < thingList.Count; ++j)
                    {
                        Thing thing = thingList[j];
                        if (thing is Pawn pawn)
                        {
                            
                            OnHit(pawn);
                        }
                        else if (thing is Building b)
                        {
                            OnHitBuilding(b);
                        }
                    }
                }
            }
        }

        private void OnHit(Pawn pawn)
        {           

            if (IsValidtarget(pawn))
            {
                WN_DefOf.ApScytheEffect.Spawn().Trigger(new TargetInfo(this.parent.Position,this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                LobCoUtility.DealPaleDamage(pawn, damage,(Pawn)this.parent);
            }
        }

        private void OnHitBuilding(Building b)
        {
            DamageInfo dinfo = new DamageInfo();
            dinfo.Def = WN_DefOf.ApScytheCut;
            dinfo.SetAmount(damage*3f);
            dinfo.SetAllowDamagePropagation(true);
            dinfo.SetIgnoreInstantKillProtection(true);
            dinfo.intendedTargetInt = b;
            AccessTools.StructFieldRefAccess<DamageInfo, float>(ref dinfo, "armorPenetrationInt") = 2.0f;
            b.TakeDamage(dinfo);
        }

        private float GetAngleCos(IntVec3 A,IntVec3 B)
        {
            IntVec3 vect1 = A - this.parent.Position;
            IntVec3 vect2 = B - this.parent.Position;
            return (vect1.x * vect2.x + vect1.z * vect2.z) / (vect1.LengthHorizontal * vect2.LengthHorizontal);
        }
        private void Die()
        {
            this.parent.Kill();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref attackcounter, "attackcounter");
            Scribe_Values.Look(ref slashcooldown, "staffcooldown");
            Scribe_Values.Look(ref target, "target");
            Scribe_Values.Look(ref btarget, "btarget");
        }
    }

    public class CompApostle : ThingComp
    {
        public Pawn WhiteNight;
        public override void PostExposeData()
        {
            base.PostExposeData();
            
            Scribe_References.Look(ref WhiteNight, "WhiteNight");
            
        }
    }

    public class CompApostleScythe : CompApostle
    {
        protected int counter = 6;
        protected int slashcooldown = 12;
        protected int attackcounter = -1;
        protected float slashrange = 5.9f;
        protected float slashanglecos = 0.5f;
        protected int delay = 2;
        protected float damage = 180f;
        protected IntVec3 target = default;
        protected IntVec3 btarget = default;

        public override void CompTick()
        {
            if (Gen.IsHashIntervalTick(this.parent, 30))
            {
                if (WhiteNight == null)
                {
                    counter--;
                }
                else
                {
                    counter = 3;
                }
                if (counter <= 0)
                {
                    //Die();
                }
                if (!(this.parent as Pawn).Downed)
                {
                    if (slashcooldown <= 0)
                    {
                        target = new IntVec3(-9999, -9999, -9999);
                        btarget = new IntVec3(-9999, -9999, -9999);
                        SlashAttackStart();
                    }
                    slashcooldown--;
                    attackcounter--;
                    if (attackcounter == 0)
                    {
                        if (target != new IntVec3(-9999, -9999, -9999))
                        {
                            SlashAttack(target);
                        }
                        else if (btarget != new IntVec3(-9999, -9999, -9999))
                        {
                            SlashAttack(btarget);
                        }
                    }
                }
            }

        }

        private void SlashAttackStart()
        {

            bool success = false;
            bool bsuccess = false;
            if (this.parent is Pawn pawn)
            {
                int numCells = GenRadial.NumCellsInRadius(slashrange - 2f);
                for (int i = 0; i < numCells; ++i)
                {
                    IntVec3 c = GenRadial.RadialPattern[i];

                    c = c + pawn.Position;
                    if (c.InBounds(pawn.Map))
                    {
                        List<Thing> thingList = c.GetThingList(pawn.Map);
                        for (int j = 0; j < thingList.Count; ++j)
                        {
                            Thing thing = thingList[j];
                            if (!success && thing is Pawn t && IsValidtarget(t))
                            {
                                target = t.Position;
                                success = true;
                                slashcooldown = 12;
                                attackcounter = delay;
                                pawn.stances.stagger.StaggerFor(delay * 3000, 0f);
                                WN_DefOf.ApWarnEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                            }
                            if(!bsuccess && thing is Building b && b.Faction!= null && b.Faction != Faction.OfAncients)
                            {
                                btarget = thing.Position;
                                bsuccess = true;
                                slashcooldown = 12;
                                attackcounter = delay;
                                pawn.stances.stagger.StaggerFor(delay * 3000, 0f);
                                WN_DefOf.ApWarnEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                            }

                        }
                    }
                }
            }
        }

        private bool IsValidtarget(Pawn pawn)
        {
            if (pawn == null || pawn.health.Dead || pawn is Apostle || pawn.HasComp<CompWhiteNight>() || !GenSight.LineOfSightToEdges(this.parent.Position, pawn.Position, this.parent.Map))
                return false;
            return true;
        }

        private void SlashAttack(IntVec3 target)
        {
            int numCells = GenRadial.NumCellsInRadius(slashrange);
            for (int i = 0; i < numCells; ++i)
            {
                IntVec3 c = GenRadial.RadialPattern[i];

                c = c + this.parent.Position;
                if (c.InBounds(this.parent.Map) && GetAngleCos(target, c) >= slashanglecos)
                {
                    float scale = (c - this.parent.Position).LengthHorizontal / 2f + 0.5f;
                    FleckMaker.Static(c, this.parent.Map, WN_DefOf.Fleck_ApSlash, scale);
                    List<Thing> thingList = c.GetThingList(this.parent.Map);
                    for (int j = 0; j < thingList.Count; ++j)
                    {
                        Thing thing = thingList[j];
                        if (thing is Pawn pawn)
                        {
                            OnHit(pawn);
                        }
                        else if (thing is Building b)
                        {
                            OnHitBuilding(b);
                        }
                    }
                }
            }
        }

        private void OnHit(Pawn pawn)
        {
            if (IsValidtarget(pawn))
            {
                WN_DefOf.ApScytheEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                if (pawn.stances != null && pawn.stances.stagger != null)
                {
                    pawn.stances.stagger.StaggerFor(240, 0.1f);
                }
                DamageInfo dinfo = new DamageInfo();
                dinfo.Def = WN_DefOf.ApScytheCut;
                dinfo.SetAmount(damage);
                dinfo.SetAllowDamagePropagation(true);
                dinfo.SetIgnoreInstantKillProtection(true);
                dinfo.intendedTargetInt = pawn;
                AccessTools.StructFieldRefAccess<DamageInfo,float>(ref dinfo,"armorPenetrationInt") = 1.1f;
                pawn.TakeDamage(dinfo);
            }
        }

        private void OnHitBuilding(Building b)
        {
            DamageInfo dinfo = new DamageInfo();
            dinfo.Def = WN_DefOf.ApScytheCut;
            dinfo.SetAmount(damage * 3f);
            dinfo.SetAllowDamagePropagation(true);
            dinfo.SetIgnoreInstantKillProtection(true);
            dinfo.intendedTargetInt = b;
            AccessTools.StructFieldRefAccess<DamageInfo, float>(ref dinfo, "armorPenetrationInt") = 1.1f;
            b.TakeDamage(dinfo);
        }

        private float GetAngleCos(IntVec3 A, IntVec3 B)
        {
            IntVec3 vect1 = A - this.parent.Position;
            IntVec3 vect2 = B - this.parent.Position;
            return (vect1.x * vect2.x + vect1.z * vect2.z) / (vect1.LengthHorizontal * vect2.LengthHorizontal);
        }
        private void Die()
        {
            this.parent.Kill();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref attackcounter, "attackcounter");
            Scribe_Values.Look(ref slashcooldown, "staffcooldown");
            Scribe_Values.Look(ref target, "target");
            Scribe_Values.Look(ref btarget, "btarget");
        }
    }

    public class CompApostleSpear : CompApostle
    {
        protected int counter = 6;
        protected int dashcooldown = 24;
        protected int attackcounter = -1;
        protected float dashrange = 36f;
        protected int delay = 4;
        protected float damage = 250f;
        protected float width = 1.2f;
        protected IntVec3 target = default;
        private Map map;
        private float minrange = 7f;


        public override void CompTick()
        {
            if (this.map == null)
            {
                this.map = this.parent.Map;
            }
            if (this.map == null)
                return;
            if (Gen.IsHashIntervalTick(this.parent, 30))
            {
                if (WhiteNight == null)
                {
                    counter--;
                }
                else
                {
                    counter = 3;
                }
                if (counter <= 0)
                {
                    //Die();
                }
                if (!(this.parent as Pawn).Downed)
                {
                    if (dashcooldown <= 0)
                    {
                        DashAttackStart();
                    }
                    dashcooldown--;
                    attackcounter--;
                    if (attackcounter == 0)
                    {
                        DashAttack(target);
                    }
                }
            }

        }

        private void DashAttackStart()
        {

            bool success = false;
            if (this.parent is Pawn pawn)
            {
                int numCells = GenRadial.NumCellsInRadius(dashrange);
                for (int i = numCells - 1; i >= 0; i--)
                {
                    IntVec3 c = GenRadial.RadialPattern[i];

                    c = c + pawn.Position;
                    if (c.InBounds(pawn.Map))
                    {
                        List<Thing> thingList = c.GetThingList(pawn.Map);
                        for (int j = 0; j < thingList.Count; ++j)
                        {
                            Thing thing = thingList[j];
                            if (!success && thing is Pawn t && IsValidtarget(t))
                            {
                                
                                    
                                target = t.Position;
                                success = true;
                                dashcooldown = 24;
                                attackcounter = delay;
                                pawn.stances.stagger.StaggerFor(delay * 3000, 0f);
                                WN_DefOf.ApWarnEffect.Spawn().Trigger(new TargetInfo(this.parent.Position, this.parent.Map), new TargetInfo(pawn.Position, pawn.Map));
                                map = t.Map;
                            }

                        }
                    }
                }
            }
        }

        private bool IsValidtarget(Pawn pawn)
        {
            if (pawn == null || pawn.health.Dead || pawn is Apostle || pawn.HasComp<CompWhiteNight>() || !GenSight.LineOfSightToEdges(this.parent.Position, pawn.Position, this.parent.Map))
                return false;
            return true;
        }

        private void DashAttack(IntVec3 target)
        {
            if (map == null)
            {
                map = this.parent.Map;
            }

            List<IntVec3> list = new List<IntVec3>();
           
            List<Pawn> hitThings = new List<Pawn>();
            
            Pawn pawn = (Pawn)this.parent;
            target = target + new IntVec3((target - pawn.Position).ToVector3() * 0.15f + 4f * (target - pawn.Position).ToVector3() / (target - pawn.Position).LengthHorizontal);
            List<IntVec3> cells = (List<IntVec3>)GenSight.BresenhamCellsBetween(this.parent.Position, target);

            
            foreach (IntVec3 cell in cells)
            {
                List<Thing> thingList = map.thingGrid.ThingsListAtFast(cell);
                foreach (Thing thing in thingList)
                {
                    if(thing.def.IsDoor && !(thing as Building_Door).Open || thing.def.passability == Traversability.Impassable || thing is Pawn && (double)(thing as Pawn).BodySize > (double)pawn.BodySize * 1.1f)
                    {
                        
                        list.Add(thing.Position);        
                        
                    }
                    
                }
            }

            

            IntVec3 closestcell = target;
            foreach (IntVec3 c in list)
            {
                if ((c - pawn.Position).LengthHorizontalSquared <= (closestcell - pawn.Position).LengthHorizontal)
                {
                    closestcell = c;
                }
            }
           
            cells = (List<IntVec3>)GenSight.BresenhamCellsBetween(this.parent.Position, closestcell);
            int ccount = cells.Count;
            for (int i = 0; i < ccount; i++)
            {
                AddAdjacent(cells, cells[i]);
            }

            


            foreach (IntVec3 cell in cells)
            {               
                if (!cell.InBounds(map))
                {
                    continue;
                }
                List<Thing> thingList = map.thingGrid.ThingsListAtFast(cell);
                foreach (Thing thing in thingList)
                {
                    if (thing is Pawn p)
                    {
                        hitThings.Add(p);
                    }

                }
            }

            ApDash dash = (ApDash)PawnFlyer.MakeFlyer(WN_DefOf.ApFlyer,pawn,closestcell,WN_DefOf.ApDashEffect, SoundDefOf.MeleeHit_Unarmed);
            dash.things = hitThings;
            dash.damage = damage;
            
            GenSpawn.Spawn((Thing)dash, this.parent.Position, map);

        }

        
        private void Die()
        {
            this.parent.Kill();
        }

        private void AddAdjacent(List<IntVec3> things,IntVec3 cell)
        {
            for (int x = -2;x<=2;x++)
            {
                for(int y = -2; y <= 2; y++)
                {
                    IntVec3 ncell = cell;
                    ncell.x += x;
                    ncell.z += y;
                    if (!things.Contains(ncell))
                    {
                        things.Add(ncell);
                    }
                }
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();          
            Scribe_Values.Look(ref attackcounter, "attackcounter");
            Scribe_Values.Look(ref dashcooldown, "staffcooldown");
            Scribe_Values.Look(ref target, "target");
        }
    }

    public class LobCoPawn : Pawn
    {
        public virtual void TakePaleDamage(float damage)
        {
            Hediff hd = this.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.PaleDamage);
            if (hd == null)
            {
                hd = this.health.AddHediff(WN_DefOf.PaleDamage);
            }
            hd.Severity += damage / 100f;
        }

        public virtual void TakeBlackDamage(float damage)
        {
            Hediff hd = this.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.BlackDamage);
            if (hd == null)
            {
                hd = this.health.AddHediff(WN_DefOf.BlackDamage);
            }
            hd.Severity += damage / 100f;
        }

        public virtual void TakeWhiteDamage(float damage)
        {
            Hediff hd = this.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.WhiteDamage);
            if (hd == null)
            {
                hd = this.health.AddHediff(WN_DefOf.WhiteDamage);
            }
            hd.Severity += damage / 100f;
        }
    }
    public class Apostle : LobCoPawn
    {
        private bool gotweapon = false;
        public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            base.PreApplyDamage(ref dinfo, out absorbed);
            if (dinfo.Amount < 6f || dinfo.ArmorPenetrationInt < 0.18f)
            {
                absorbed = true;
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (this.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.Aleph) == null)
            {
                this.health.AddHediff(WN_DefOf.Aleph);
            }
            ThingWithComps tth;
            
            if (this.HasComp<CompApostleGuardian>())
            {
                tth = (ThingWithComps)ThingMaker.MakeThing(WN_DefOf.ApGuardianWeapon);

            }
            else if (this.HasComp<CompApostleScythe>())
            {
                tth = (ThingWithComps)ThingMaker.MakeThing(WN_DefOf.ApScytheWeapon);

            }
            else if (this.HasComp<CompApostleSpear>())
            {
                tth = (ThingWithComps)ThingMaker.MakeThing(WN_DefOf.ApSpearWeapon);

            }
            else if (this.HasComp<CompApostleStaff>())
            {
                tth = (ThingWithComps)ThingMaker.MakeThing(WN_DefOf.ApStaffWeapon);

            }
            else tth = null;
            if (!this.equipment.Contains(tth) && tth != null && !gotweapon)
            {
                this.equipment.AddEquipment(tth);
                gotweapon = true;
            }          
            
            
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref gotweapon,"gotweapon");
        }
        public override void TakePaleDamage(float damage)
        {
            DamageInfo dinfo = new DamageInfo();
            dinfo.Def = DamageDefOf.Blunt;
            dinfo.SetAmount(damage);
            dinfo.SetAllowDamagePropagation(true);
            dinfo.intendedTargetInt = this;
            dinfo.SetIgnoreArmor(true);
            this.TakeDamage(dinfo);
        }

        public override void TakeWhiteDamage(float damage)
        {
            DamageInfo dinfo = new DamageInfo();
            dinfo.Def = DamageDefOf.Blunt;
            dinfo.SetAmount(damage);
            dinfo.SetAllowDamagePropagation(true);
            dinfo.intendedTargetInt = this;
            dinfo.SetIgnoreArmor(true);
            this.TakeDamage(dinfo);
        }
    }

    public class LobCoArmor : ThingComp
    {
        public virtual float CutPaleDamage(float damage)
        {
            return damage;
        }

        public virtual float CutBlackDamage(float damage)
        {
            return damage;
        }
        public virtual float CutWhiteDamage(float damage)
        {
            return damage;
        }

        public virtual void PrePreApplyDamage(ref DamageInfo dinfo,out bool absorbed)
        {
            absorbed = false;
        }
        
    }
    public class CompPDLArmor : LobCoArmor
    {
        public float shield;
        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref shield, "shield");

        }

        public override void PrePreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
        {
            //Log.Message("----- " + "shield = " + shield);
            //Log.Message("damage = " + dinfo.Amount);
            if ((dinfo.Amount < 5f) || (HasGift() && dinfo.Amount < 10f))
            {
                absorbed=true;
            }
            else
            {
                absorbed = false;
            }
            float dmg = dinfo.Amount * 0.2f;
            dinfo.SetAmount(Mathf.Clamp(dinfo.Amount * 0.2f - shield,0f,float.MaxValue));
            //Log.Message("post reduce damage = " + dinfo.Amount);
            shield = Mathf.Clamp(shield - dmg,0f,float.MaxValue);
            //Log.Message("post reduce shield = " + shield + " -----");
        }

        private bool HasGift()
        {;
            if (this.parent is Pawn pawn && pawn.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.WNegoGift) != null)
            {
                return true;
            }
            return false;
        }
        public override float CutPaleDamage(float damage)
        {
            //Log.Message("--P-- " + "shield = " + shield);
            //Log.Message("damage = " + damage);
            if ((damage < 5f) || (HasGift() && damage < 10f))
                return 0f;
            float dmg = damage * 0.2f;
            damage = Mathf.Clamp(damage * 0.2f - shield, 0f, float.MaxValue);
            shield = Mathf.Clamp(shield - dmg,0f,float.MaxValue);
            ///Log.Message("post reduce damage = " + damage);
            //Log.Message("post reduce shield = " + shield + " --P--");
            return damage;
        }

        public override float CutBlackDamage(float damage)
        {
            if ((damage < 5f) || (HasGift() && damage < 10f))
                return 0f;
            float dmg = damage * 0.2f;
            damage = Mathf.Clamp(damage * 0.2f - shield, 0f, float.MaxValue);
            shield = Mathf.Clamp(shield - dmg, 0f, float.MaxValue);
            return damage;
        }

        public override float CutWhiteDamage(float damage)
        {
            if ((damage < 5f) || (HasGift() && damage < 10f))
                return 0f;
            float dmg = damage * 0.2f;
            damage = Mathf.Clamp(damage * 0.2f - shield, 0f, float.MaxValue);
            shield = Mathf.Clamp(shield - dmg, 0f, float.MaxValue);
            return damage;
        }
    }
    public class WhiteNight : LobCoPawn
    {
        public List<Pawn> Apostles;
        private float[] levels => new float[] {0.3f,0.3f,0.35f,0.4f,0.45f};
        public List<Pawn> ApostlesReborn;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            if (Apostles == null)
            {
                Apostles = new List<Pawn>();
            }
            if (ApostlesReborn == null)
            {
                ApostlesReborn = new List<Pawn>();
            }
            if (this.Dead)
            {
                //Log.Message("wn ded on spawn");
            }

            if (map == null || this == null)
            {
                //Log.Message("aaaaaaaaaabbb");
            }
            base.SpawnSetup (map, respawningAfterLoad);
            
        }

        public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
        {
            Hediff h = this.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.QliphothWN);
            if (h != null)
            {
                h.Severity += 1;
            }
            h = studier.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.WNegoGift);
            if (h == null && Rand.Chance(0.01f))
            {
                studier.health.AddHediff(WN_DefOf.WNegoGift);
            }
            LobCoUtility.DealPaleDamage(studier,GetWorkDamage(studier),this);
        }

        private int GetWorkDamage(Pawn studier)
        {
            int dmg = 0;
            float s1 = studier.skills.GetSkill(SkillDefOf.Intellectual).GetLevel();
            if (studier.Inhumanized())
            {
                s1 = s1 * 1.1f + 1f;
            }
            float s2 = studier.skills.GetSkill(SkillDefOf.Social).GetLevel();
            if (studier.Inhumanized())
            {
                s2 = (s2 - 1f) / 1.1f;
            }
            float sf = Mathf.Max(s1, s2);
            int level = Mathf.Clamp((int)sf / 5,0,4);
            float chance = levels[level] * ((40f + sf) / 40f) * Mathf.Sqrt(studier.GetStatValue(StatDefOf.StudyEfficiency));
            //Log.Message(chance);
            for (int i = 0;i<35;i++)
            {
                if (!Rand.Chance(chance))
                {
                    dmg += 7;
                }
            }
            return dmg;
        }
        public override void TakePaleDamage(float damage)
        {
            DamageInfo dinfo = new DamageInfo();
            dinfo.Def = DamageDefOf.Blunt;
            dinfo.SetAmount(damage);
            dinfo.SetAllowDamagePropagation(true);
            dinfo.intendedTargetInt = this;
            dinfo.SetIgnoreArmor(true);
            this.TakeDamage(dinfo);
        }

        public override void TakeWhiteDamage(float damage)
        {
            DamageInfo dinfo = new DamageInfo();
            dinfo.Def = DamageDefOf.Blunt;
            dinfo.SetAmount(damage);
            dinfo.SetAllowDamagePropagation(true);
            dinfo.intendedTargetInt = this;
            dinfo.SetIgnoreArmor(true);
            this.TakeDamage(dinfo);
        }

        public static void Advent(PlagueDoctor pd, WhiteNight whitenight)
        {          
            
            if (Find.EntityCodex != null && !Find.EntityCodex.Discovered(WN_DefOf.WN_Entry))
            {
                Find.EntityCodex.SetDiscovered(WN_DefOf.WN_Entry);
            }
            if (whitenight == null)
            {
                Map mp = pd.Map;
                if (mp == null)
                {
                    mp = pd.MapHeld;

                }
                if(mp == null)
                {
                    Log.Warning("plaguedoctor has no map");
                }
                PawnGenerationRequest pgr = new PawnGenerationRequest(WN_DefOf.WhiteNight, Faction.OfEntities, PawnGenerationContext.NonPlayer, fixedBiologicalAge: 0f, fixedChronologicalAge: 666f);


                Pawn wn = PawnGenerator.GeneratePawn(pgr);
                wn = (Pawn)GenSpawn.Spawn(wn, pd.Position, mp, (WipeMode)0);
                wn.GetComp<CompWhiteNight>().dropego = true;
                WhiteNight w = wn as WhiteNight;
                if (pd == null)
                {
                    Log.Message("no plaguedoctor");
                    return; 
                }
                if (pd.Apostles == null)
                {
                    Log.Message("no plaguedoctor apostles");
                    return;
                }
                if (w == null)
                {
                    Log.Message("no whitenight spawned");
                    return;
                }
                if (w.Apostles == null)
                {
                    Log.Message("no whitenight apostles");
                    return;
                }
                foreach (Pawn apostle in pd.Apostles)
                {
                    w.Apostles.Add(apostle);
                }
                for (int i = 0; i < 10; i++)
                {
                    bool flag = true;
                    Pawn apostle;
                    if (pd.Apostles.Count <= i)
                    {
                        //Log.Message("apostles count < " + i);
                        apostle = pd;                  
                        flag = false;
                    }
                    else
                    {
                        apostle = pd.Apostles[i];
                    }
                    
                    if (apostle == null)
                    {
                        //Log.Mess("no apostle " + (i + 1).ToString());
                        apostle = pd;
                        flag = false;
                    }
                    Pawn ap = null;
                    Name name = apostle.Name;
                    float age = apostle.ageTracker.AgeBiologicalYearsFloat;
                    pgr.FixedBiologicalAge = age;
                    if (i == 0)
                    {   
                        pgr.KindDef = WN_DefOf.FirstApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 1)
                    {
                        pgr.KindDef = WN_DefOf.SecondApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 2)
                    {
                        pgr.KindDef = WN_DefOf.ThirdApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 3)
                    {
                        pgr.KindDef = WN_DefOf.FourthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 4)
                    {
                        pgr.KindDef = WN_DefOf.FifthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 5)
                    {
                        pgr.KindDef = WN_DefOf.SixthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 6)
                    {
                        pgr.KindDef = WN_DefOf.SeventhApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 7)
                    {
                        pgr.KindDef = WN_DefOf.EighthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 8)
                    {
                        pgr.KindDef = WN_DefOf.NinthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 9)
                    {
                        pgr.KindDef = WN_DefOf.TenthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 10)
                    {
                        pgr.KindDef = WN_DefOf.EleventhApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                        
                    }
                    if (!apostle.Destroyed && flag)
                    {
                        apostle.health.AddHediff(WN_DefOf.WN_whatthesigma);
                        //apostle.Destroy(DestroyMode.Vanish);
                    }
                    if (apostle.Map != null)
                    {
                        mp = apostle.Map;
                    }
                    Pawn apr = (Pawn)GenSpawn.Spawn(ap, apostle.Position, mp);
                    apr.Name = name;
                    w.ApostlesReborn.Add(apr);
                }
            }

            else 
            {
                whitenight.health.forceDowned = false;
                //whitenight.health.Reset();
                PawnGenerationRequest pgr = new PawnGenerationRequest(WN_DefOf.WhiteNight, Faction.OfEntities, PawnGenerationContext.NonPlayer, fixedBiologicalAge: 0f, fixedChronologicalAge: 666f);
                Map mp = whitenight.Map;
                if (mp == null)
                {
                    mp = whitenight.MapHeld;
                }
                if (mp == null)
                {
                    Log.Warning("whitenight has no map");
                }
                if (whitenight.Apostles == null)
                {
                    Log.Warning("wn has no apostles list");
                }
                for (int i = 0; i < 10; i++)
                {
                    bool flag = true;
                    Pawn apostle;
                    if (whitenight.Apostles.Count <= i)
                    {
                        //Log.Warning("apostles count < " + i);
                        apostle = whitenight;
                        flag = false;
                    }
                    else
                    {
                        apostle = whitenight.Apostles[i];
                    }
                    
                    if (apostle == null)
                    {
                        //Log.Warning("no apostle " + (i + 1).ToString());
                        flag = false;
                        apostle = whitenight;
                    }
                    Pawn ap = null;
                    
                    
                    float age = apostle.ageTracker.AgeBiologicalYearsFloat;
                    pgr.FixedBiologicalAge = age;
                    if (i == 0)
                    {
                        pgr.KindDef = WN_DefOf.FirstApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 1)
                    {
                        pgr.KindDef = WN_DefOf.SecondApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 2)
                    {
                        pgr.KindDef = WN_DefOf.ThirdApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 3)
                    {
                        pgr.KindDef = WN_DefOf.FourthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 4)
                    {
                        pgr.KindDef = WN_DefOf.FifthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 5)
                    {
                        pgr.KindDef = WN_DefOf.SixthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 6)
                    {
                        pgr.KindDef = WN_DefOf.SeventhApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 7)
                    {
                        pgr.KindDef = WN_DefOf.EighthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 8)
                    {
                        pgr.KindDef = WN_DefOf.NinthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 9)
                    {
                        pgr.KindDef = WN_DefOf.TenthApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);
                    }
                    if (i == 10)
                    {
                        pgr.KindDef = WN_DefOf.EleventhApostle;
                        ap = PawnGenerator.GeneratePawn(pgr);

                    }
                    if (!apostle.Destroyed && flag)
                    {
                        apostle.health.AddHediff(WN_DefOf.WN_whatthesigma);
                    }
                    if (apostle.Map != null)
                    {
                        mp = apostle.Map;
                    }
                    Pawn apr = (Pawn)GenSpawn.Spawn(ap, apostle.Position, mp);
                    if (apostle.Name != null && flag)
                    {
                        apr.Name = apostle.Name;
                    }
                    whitenight.ApostlesReborn.Add(apr);

                }
            }
            if (pd != null && !pd.Destroyed)
            {
                pd.Destroy();
            }
            
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref this.Apostles, "Apostles", false, LookMode.Reference);
            Scribe_Collections.Look(ref this.ApostlesReborn, "ApostlesReborn", false, LookMode.Reference);
        }
    }
    public class HediffCompProperties_QliphothCounter : HediffCompProperties
    {
        public HediffCompProperties_QliphothCounter() => this.compClass = typeof(CompQlipothCounter);
    }

    public class HediffCompProperties_wts : HediffCompProperties
    {
        public HediffCompProperties_wts() => this.compClass = typeof(CompWhatTheSigma);
    }

    public class HediffCompProperties_Blessing : HediffCompProperties
    {
        public HediffCompProperties_Blessing() => this.compClass = typeof(CompBlessing);
    }

    public class CompBlessing : HediffComp
    {
        public PlagueDoctor Doctor;
        public int Order;
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref Doctor, "Doctor");
            Scribe_Values.Look(ref Order, "Order");
        }
    }
    public class HediffCompProperties_BlackDamage : HediffCompProperties
    {
        public HediffCompProperties_BlackDamage() => this.compClass = typeof(CompBlackDamage);
    }

    public class HediffCompProperties_WhiteDamage : HediffCompProperties
    {
        public HediffCompProperties_WhiteDamage() => this.compClass = typeof(CompWhiteDamage);
    }

    public class CompBlackDamage : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (this.parent == null || this.parent.pawn == null)
            {
                return;
            }
            severityAdjustment = -0.0003f;
            if (Gen.IsHashIntervalTick((Thing)this.parent.pawn, 300))
            {
                Pawn pawn = this.parent.pawn;
                DamageInfo dinfo = new DamageInfo();
                dinfo.Def = WN_DefOf.BlackSubDamage;
                dinfo.SetAmount(this.parent.Severity * 10f);
                dinfo.SetAllowDamagePropagation(true);
                dinfo.intendedTargetInt = pawn;
                dinfo.SetIgnoreArmor(true);
                pawn.TakeDamage(dinfo);
            }
            
        }

        
    }
    public class CompWhiteDamage : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (this.parent == null || this.parent.pawn == null)
            {
                return;
            }
            severityAdjustment = -0.0003f;
            if (Gen.IsHashIntervalTick(this.parent.pawn, 300))
            {               
                Pawn pawn = this.parent.pawn;
                if (this.parent.Severity == 0)
                {
                    return;
                }
                if (Rand.Chance(0.05f * this.parent.Severity))
                {
                    MentalStateUtility.StartMentalState(pawn, WN_DefOf.MSSuicide);
                }
                else if (Rand.Chance(0.1f * this.parent.Severity))
                {
                    MentalStateUtility.StartMentalState(pawn, MentalStateDefOf.Berserk);
                }
                
            }
        }
        

    }

    public class CompWhatTheSigma : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (this.parent == null || this.parent.pawn == null)
            {
                return;
            }
            severityAdjustment = -0.02f;
            
        }

        public override void Notify_PawnKilled()
        {
            if (!this.parent.pawn.Destroyed)
                this.parent.pawn.Destroy();
            base.Notify_PawnKilled();
        }
        public override void CompPostPostRemoved()
        {
            if (!this.parent.pawn.Destroyed)
                this.parent.pawn.Destroy();
            base.CompPostPostRemoved();
        }


    }

    public class MentalState_Suicide : MentalState
    {
        private float attackcooldown;
        public override bool ForceHostileTo(Thing t)
        {
            if (t is Pawn pawn && pawn == this.pawn)
            {
                return true;
            }
            return false;
        }

        public override void PostStart(string reason)
        {
            base.PostStart(reason);
            attackcooldown = this.pawn.Tools[0].cooldownTime;
        }
        public override void MentalStateTick()
        {
            base.MentalStateTick();
            attackcooldown -= (1f/60f);
            if (attackcooldown <= 0f)
            {
                attackcooldown = this.pawn.Tools[0].cooldownTime;
                DamageInfo dinfo = new DamageInfo();
                dinfo.SetAmount(this.pawn.Tools[0].power);
                dinfo.Def = DamageDefOf.Cut;
                dinfo.SetIgnoreArmor(true);
                this.pawn.TakeDamage(dinfo);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref attackcooldown, "attackcooldown");          
        }
    }
    public class ApDash : PawnFlyer
    {
        public float damage;
        public List<Pawn> things;
        public float width;
        public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
        {
            Pawn pawn = this.FlyingPawn;
            base.DynamicDrawPhaseAt(phase, drawLoc, flip);
            
        }

        public override void Tick()
        {
            base.Tick();

            if (this.Map != null)
            {
                WN_DefOf.ApDashEffect.Spawn().Trigger(new TargetInfo(this.Position, this.Map), new TargetInfo(this.Position, this.Map));
            }
            while (things.Count > 0)
            {
                Pawn pawn = things[0];
                //if ((pawn.Position - this.Position).LengthHorizontalSquared <= width*width)
                //{
                if (pawn != null)
                {
                    OnHit(pawn);
                    //}
                    things.Remove(pawn);
                }
            }

        }

        private void OnHit(Pawn pawn)
        {
            if (IsValidtarget(pawn))
            {
                LobCoUtility.DealBlackDamage(pawn, damage,this.FlyingPawn);
                Map m = pawn.Map;
                if (m == null)
                {
                    m = this.Map;
                }
                if(m != null && this.Map != null)
                    WN_DefOf.ApSpearEffect.Spawn().Trigger(new TargetInfo(this.Position, this.Map), new TargetInfo(pawn.Position, m));
                if (pawn.stances != null && pawn.stances.stagger != null)
                {
                    pawn.stances.stagger.StaggerFor(200, 0.1f);
                }
                
                DamageInfo dinfo = new DamageInfo();
                dinfo.Def = WN_DefOf.ApSpearCut;
                dinfo.SetAmount(damage);
                dinfo.SetAllowDamagePropagation(true);
                dinfo.SetIgnoreInstantKillProtection(true);
                dinfo.intendedTargetInt = pawn;
                AccessTools.StructFieldRefAccess<DamageInfo, float>(ref dinfo, "armorPenetrationInt") = 1.6f;
                pawn.TakeDamage(dinfo);
            }
        }

        private bool IsValidtarget(Pawn pawn)
        {
            if (pawn == null || pawn is Apostle || pawn.HasComp<CompWhiteNight>())
                return false;
            return true;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.damage, "damage", 0, false);
            Scribe_Collections.Look(ref this.things, "things");

        }

    }
    public class Resurrection : Hediff
    {
        private int timer = 1;
        public void ProgressResurrection()
        {
            
            if (this.pawn == null)
            {
                return;
                
            }
            if (timer <= 0)
            {
                
                var container = pawn.ParentHolder;
                while (container is Corpse)
                {
                    container = container.ParentHolder;
                }
                if (container is Building_Casket grave)
                {
                    grave.EjectContents();
                }
                
                bool success = ResurrectionUtility.TryResurrect(pawn);
                if (!success)
                {
                    Log.Warning("failed to resurrect");
                }
                pawn.health.RemoveAllHediffs();
                pawn.health.AddHediff(WN_DefOf.Aleph);
                pawn.health.AddHediff(WN_DefOf.QliphothWN);
            }
            timer--;
        }

        

        public override void ExposeData()
        {
            Scribe_Values.Look(ref timer, "timer");
            base.ExposeData();
        }
    }
    public class CompQlipothCounter : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (this.parent == null || this.parent.pawn == null)
            {
                return;
            }
            if (Gen.IsHashIntervalTick((Thing)this.parent.pawn, 60))
            {
                severityAdjustment = -0.003f;
                Pawn pawn = this.parent.pawn;
                if (pawn.HasComp<CompHoldingPlatformTarget>())
                {
                    if (!pawn.GetComp<CompHoldingPlatformTarget>().CurrentlyHeldOnPlatform)
                    {
                        severityAdjustment = -0.03f;
                    }
                    float mincontainment = 300f;//pawn.GetStatValue(StatDefOf.MinimumContainmentStrength);
                    if (pawn.GetComp<CompHoldingPlatformTarget>().EntityHolder == null)
                    {
                        severityAdjustment = -0.03f;
                        return;
                    }
                    float containment = pawn.GetComp<CompHoldingPlatformTarget>().EntityHolder.ContainmentStrength;
                    if (containment < mincontainment)
                    {
                        severityAdjustment = -0.015f * (1 + (containment / mincontainment));
                    }

                }
                severityAdjustment *= 0.5f;
            }
        }
  
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            Pawn pawn = this.parent.pawn;
            if (pawn.HasComp<CompHoldingPlatformTarget>() && pawn.GetComp<CompHoldingPlatformTarget>().EntityHolder == null && pawn.GetComp<CompHoldingPlatformTarget>().CurrentlyHeldOnPlatform)
            {
                pawn.GetComp<CompHoldingPlatformTarget>().Escape(true);
                if (pawn is WhiteNight)
                {
                    pawn.GetComp<CompWhiteNight>().dropego = true;
                    WhiteNight.Advent(null,pawn as WhiteNight);
                }
            }
        }
    }

    public class CompProperties_WhiteNight : CompProperties
    {
        public CompProperties_WhiteNight() => this.compClass = typeof(CompWhiteNight);
    }
    public class CompWhiteNight : ThingComp
    {


        public bool dropego;

        public int cdleft = -1;
       
        
        public override void CompTick()
        {
            Pawn pawn = (Pawn)this.parent;
            if (cdleft == -1)
            {
                cdleft = 300;
                
                pawn.health.AddHediff(WN_DefOf.Aleph);
            }
            else if (cdleft == 0)
            {
                SpawnRing(pawn);
                cdleft = 6000;
            }
            if (pawn.health.hediffSet.GetFirstHediffOfDef(WN_DefOf.QliphothWN) == null)
            {
                cdleft--;
            }
            
        }

        private void SpawnRing(Pawn pawn)
        {
            Ring ring = (Ring)ThingMaker.MakeThing(WN_DefOf.WN_Ring, (ThingDef)null);
            ring = (Ring)GenSpawn.Spawn((Thing)ring, pawn.Position, pawn.Map, (WipeMode)0);
            ring.source = this.parent as WhiteNight;
            WN_DefOf.RingEffect.Spawn().Trigger(new TargetInfo(pawn.Position, pawn.Map),new TargetInfo(pawn.Position, pawn.Map));
        }

        public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
        {
            base.Notify_Killed(prevMap, dinfo);
            if (this.parent != null)
            {
                Pawn pawn = (Pawn)this.parent;
                //ResurrectionUtility.TryResurrect(pawn);
                pawn.health.AddHediff(WN_DefOf.AbnormalityResurrection);
            }
            if (dropego)
            {
                Thing ego = ThingMaker.MakeThing(WN_DefOf.PDLArmor);
                GenSpawn.Spawn(ego, this.parent.Position, prevMap, (WipeMode)0);
                ego = ThingMaker.MakeThing(WN_DefOf.ParadiseLostWeapon);
                GenSpawn.Spawn(ego, this.parent.Position, prevMap, (WipeMode)0);
                dropego = false;
            }
            if ((this.parent as WhiteNight).ApostlesReborn == null)
            {
                //Log.Warning("no ApRe list");
                return;
            }
            foreach (Pawn apostle in (this.parent as WhiteNight).ApostlesReborn)
            {
                if (apostle != null && !apostle.Destroyed && !apostle.Dead)    
                    apostle.Kill(null);
                
            }
            (this.parent as WhiteNight).ApostlesReborn.Clear();

        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref this.cdleft, "cdleft", 0, false);            
            Scribe_Values.Look<bool>(ref this.dropego, "dropego");
        }

    }
    /*
    public class JobGiver_RingAttack : JobGiver_AICastAbility
    {
        protected override LocalTargetInfo GetTarget(Pawn pawn, Ability ability)
        {
            return new LocalTargetInfo();
        }
    }
    public class CompProperties_RingAttack : CompProperties_AbilityEffect
    {
        public CompProperties_RingAttack()
        {
            ((AbilityCompProperties)this).compClass = typeof(CompAbilityEffect_RingAttack);
        }
    }

    public class CompAbilityEffect_RingAttack : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = this.parent.pawn;
            Ring ring = (Ring)ThingMaker.MakeThing(WN_DefOf.WN_Ring, (ThingDef)null);
            GenSpawn.Spawn((Thing)ring, pawn.Position, pawn.Map, (WipeMode)0);
        }
    }
    */
    
    [StaticConstructorOnStartup]
    public class Ring : Thing
    {
        private static readonly MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();
        private static readonly Material RingMaterial = MaterialPool.MatFrom("Effects/Ring", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.Tornado);
        public float maxRadius = 56f;
        private int age;
        public float speed = 6f;
        private int prevRad;
        private float damage = 40f;
        public WhiteNight source;

        private HashSet<Thing> hitThings;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                this.age = 0;
                prevRad = 0;
            }
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            float r = GetRadius();
            drawLoc.y = Altitudes.AltitudeFor((AltitudeLayer)25);
            Matrix4x4 matrix = Matrix4x4.TRS(drawLoc, Quaternion.identity, new Vector3(r * 2f, 1f, r * 2f));
            //Ring.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, Color.red);
            Graphics.DrawMesh(MeshPool.plane10, matrix, RingMaterial, 0,(Camera) null, 0, Ring.matPropertyBlock);
        }

        public override void Tick()
        {
            this.age++;
            if (Gen.IsHashIntervalTick((Thing)this, 4))
            {
                

                int r = (int)GetRadius();
                if (GetRadius() >= maxRadius)
                {
                    ((Thing)this).Destroy((DestroyMode)0);
                }
                else if (r != prevRad)
                {
                    DamageRing(r);
                    prevRad = r;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.age, "age", 0, false);
            Scribe_References.Look(ref this.source, "source");
        }

        private void DamageRing(int radius)
        {
            
            int numCells = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < numCells; ++i)
            {
                IntVec3 c = GenRadial.RadialPattern[i];
                if ((double)c.LengthHorizontal >= (double)prevRad - 0.5d)
                {
                    c = c + this.Position;
                    if (c.InBounds(this.Map))
                    {
                        List<Thing> thingList = c.GetThingList(this.Map);
                        for (int j = 0; j < thingList.Count; ++j)
                        {
                            Thing thing = thingList[j];
                            
                            if (hitThings == null)
                                hitThings = new HashSet<Thing>();
                            if (!hitThings.Contains(thing))
                            {
                                hitThings.Add(thing);
                                OnHit(thing);
                            }

                            else
                                continue;
                            

                        }
                    }
                }
            }

        }

        private void OnHit(Thing thing)
        {
            if (thing != null && thing is Pawn hitPawn)
            {
                bool wn = false;
                bool mech = false;
                bool apostle = false;

                if (hitPawn.def.defName == "BaseMechanoid")
                {
                    mech=true;
                }
                if (hitPawn is Apostle)
                {
                    apostle = true;
                }

                if (hitPawn.HasComp<CompWhiteNight>())
                {
                    wn = true;
                }

                foreach (var hediff in hitPawn.health.hediffSet.hediffs)
                {
                    if (hediff.def.isBad)
                    {
                        float mult = 1f;
                        if(wn)
                        {
                            mult = -10f;     
                        }

                        if(apostle)
                        {
                            mult = -2f;
                        }                        

                        hediff.Severity = hediff.Severity + 0.08f * mult;
                    }
                }
                if (!wn && !mech && !apostle)
                {
                    LobCoUtility.DealPaleDamage(hitPawn,damage,this.source);
                }
            }
        }

        private float GetRadius()
        {
            float a = (age / 60f) * this.speed;
            return ((Mathf.Sqrt(a)+ a) / (Mathf.Sqrt(a) + a + 12f)) * this.maxRadius * 1.3f;
        }
    }

    
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony(id: "rimworld.doge.wn");

            harmony.Patch(AccessTools.Method(typeof(Corpse), nameof(Pawn.TickRare)), postfix: new HarmonyMethod(patchType, nameof(CorpsePostfix)));
            harmony.Patch(AccessTools.Method(typeof(Pawn), nameof(Pawn.PreApplyDamage)), prefix: new HarmonyMethod(patchType, nameof(PawnPrefix)));
        }

        public static void CorpsePostfix(Corpse __instance)
        {
            if (__instance == null || __instance.InnerPawn == null) return;
            Pawn pawn = __instance.InnerPawn;
            if (pawn.Dead)
            {
                for (int i = pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
                {
                    var hediff = pawn.health.hediffSet.hediffs[i];
                    if (hediff is Resurrection res)
                    {
                        res.ProgressResurrection();
                        break;
                    }
                    if (hediff.TryGetComp<CompWhatTheSigma>() != null && !__instance.Destroyed)
                    {
                        __instance.Destroy();
                    }
                }
            }
        }

        public static void PawnPrefix(Pawn __instance, ref DamageInfo dinfo, out bool absorbed)
        {
            //Log.Message("prefix start");
            bool abs = false;
            if (__instance.apparel == null)
            {
                absorbed = false;
                return;
            }
            foreach (Thing app in __instance.apparel.WornApparel)
            {
                if (app.HasComp<LobCoArmor>())
                {
                    app.TryGetComp<LobCoArmor>().PrePreApplyDamage(ref dinfo,out abs);
                    if (abs)
                    {
                        //Log.Message("absorbed damage " + dinfo.Amount);
                        dinfo.SetAmount(0);
                        absorbed = true;
                    }
                }
            }
            absorbed = false;
        }
    }
}
