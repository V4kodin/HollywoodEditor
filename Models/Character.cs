using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HollywoodEditor.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Character
    {
        public static List<string> Labels => new List<string>()
        {
            "HARDWORKING",
            "LAZY",
            "DISCIPLINED",
            "UNDISCIPLINED",
            "PERFECTIONIST",
            "INDIFFERENT",
            "HOTHEADED",
            "CALM",
            "LEADER",
            "TEAM_PLAYER",
            "OPEN_MINDED",
            "RACIST",
            "MISOGYNIST",
            "XENOPHOBE",
            "DEMANDING",
            "MODEST",
            "ARROGANT",
            "SIMPLE",
            "HEARTBREAKER",
            "CHASTE",
            "CHEERY",
            "MELANCHOLIC",
            "ALCOHOLIC",
            "LUDOMANIAC",
            "JUNKIE",
            "UNWANTED_ACTOR",
            "UNTOUCHABLE",
            "STERILE",
            "IMAGE_VIVID",
            "IMAGE_SOPHISTIC",
            "IMMORTAL",
            "SUPER_IMMORTAL"
        };

        private int age;
        private string birthDate1;
        private string normalFirst1;
        private string normalLast1;
        private string customName1;
        private bool calcages = false;
        private string myCustomName = null;
        private string studioId1;
        private bool isDead;
        private DateTime CurrNow = new DateTime();
        private double limit1;
        [JsonIgnore]
        public bool IsInit { get; set; }

        public double limit
        {
            get => limit1;
            set
            {
                limit1 = value;
                if (professions != null)
                    if (professions.Value > limit1)
                        professions.Value = limit1;
            }
        }
        public double mood { get; set; }
        public double attitude { get; set; }
        public int id { get; set; }
        public int portraitBaseId { get; set; }
        public string firstNameId { get; set; }
        public string lastNameId { get; set; }
        public string birthDate
        {
            get => birthDate1;
            set
            {
                birthDate1 = value;
            }
        }
        public string studioId
        {
            get
            {
                if (studioId1 == null)
                    studioId1 = "NONE";
                return studioId1;
            }
            set
            {
                if (value == "NONE") //убираем студию
                {
                    studioId1 = null;
                    contract = null;
                    state = 0;
                    if (IsDead)
                        state = 16;
                }
                else
                {
                    if (studioId != null && value != null) //меням студию
                    {
                        if (value == "PL")
                            state = 1026;
                        else //if (state == 1026)
                            state = 36;
                        if (IsDead)
                            state = 20;
                        if (!IsInit)
                            if (contract == null)
                            {
                                contract = new Contract(CurrNow);
                            }
                    }
                    studioId1 = value;
                }

            }
        }
        public int state { get; set; }
        //1 = F, 0 = M
        public int gender { get; set; }
        [JsonIgnore]
        public Professions professions { get; set; }
        public Contract contract { get; set; }
        [JsonIgnore]
        public ObservableCollection<WhiteTag> whiteTagsNEW { get; set; }
        public List<string> aSins { get; set; }
        public ObservableCollection<string> labels { get; set; }
        public string deathDate { get; set; }
        public int causeOfDeath { get; set; }
        [JsonIgnore]
        public bool IsBusyOnJob { get; set; }
        public Character()
        {
            IsInit = true;
            whiteTagsNEW = new ObservableCollection<WhiteTag>();
            labels = new ObservableCollection<string>();
            aSins = new List<string>();
        }

        #region custom
        public string JsonString { get; set; }
        public string normalFirst
        {
            get
            {
                if (string.IsNullOrWhiteSpace(normalFirst1))
                    return firstNameId;
                else
                    return normalFirst1;
            }
            set => normalFirst1 = value;
        }
        public string normalLast
        {
            get
            {
                if (string.IsNullOrWhiteSpace(normalLast1))
                    return lastNameId;
                else
                    return normalLast1;
            }
            set => normalLast1 = value;
        }
        public string MyCustomName
        {
            get
            {
                if (myCustomName == null)
                    return customName;
                else
                    return myCustomName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                myCustomName = value;
            }
        }
        public string customName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(customName1))
                    return $"{normalFirst} {normalLast}";
                else
                    return customName1;
            }
            set { customName1 = value; }
        }
        public bool CustomNameWasSetted => MyCustomName != customName;
        public DateTime GetBirthDate => DateTime.ParseExact(birthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        public bool IsDead
        {
            get => isDead;
            set
            {
                isDead = value;
                if (!value)
                {
                    deathDate = "01-01-0001";
                    causeOfDeath = 0;
                    if (ReservState != 16 && ReservState != 20)
                    {
                        state = ReservState;
                    }
                    else
                    {
                        if (studioId == "PL")
                            state = 1026;
                        else if (studioId == "NONE")
                            state = 0;
                        else
                            state = 36;
                    }

                }
                else
                {
                    deathDate = ReservDateOfDeath;
                    causeOfDeath = ReservCauseOfDeath;
                    if (studioId == "NONE")
                        state = 16;
                    else
                        state = 20;
                }
            }
        }
        public string ReservDateOfDeath = string.Empty;
        public int ReservCauseOfDeath = 0;
        public int ReservState = 64;
        public int Age
        {
            get => age;
            set
            {
                if (!calcages)
                    birthDate = GetBirthDate.AddYears(age - value).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                else
                    calcages = false;
                age = value;
            }
        }
        public void SetFullAge(DateTime now)
        {
            var age = now.Year - GetBirthDate.Year;
            CurrNow = now;
            if (GetBirthDate.Date > now.AddYears(-age)) age--;
            calcages = true;
            Age = age;
        }
        public string ImgPath
        {
            get
            {
                string a = $"{App.PathToExe}Resources\\Profiles\\";
                a += $"PRT_";

                switch (professions.GetProfession)
                {
                    case Professions.Profession.Actor:
                    case Professions.Profession.Composer:
                    case Professions.Profession.Scriptwriter:
                    case Professions.Profession.Cinematographer:
                    case Professions.Profession.FilmEditor:
                    case Professions.Profession.Producer:
                    case Professions.Profession.Director:
                    case Professions.Profession.Else:
                        a += "TALENT_";
                        break;
                    case Professions.Profession.Agent:
                        a += "AGENT_";
                        break;
                    case Professions.Profession.LieutScript:
                    case Professions.Profession.LieutPrep:
                    case Professions.Profession.LieutProd:
                    case Professions.Profession.LieutPost:
                    case Professions.Profession.LieutRelease:
                    case Professions.Profession.LieutSecurity:
                    case Professions.Profession.LieutProducers:
                    case Professions.Profession.LieutInfrastructure:
                    case Professions.Profession.LieutTech:
                    case Professions.Profession.LieutMuseum:
                    case Professions.Profession.LieutEscort:
                    case Professions.Profession.CptHR:
                    case Professions.Profession.CptLawyer:
                    case Professions.Profession.CptFinancier:
                    case Professions.Profession.CptPR:
                        a += "LIEUT_";
                        break;
                }

                if (gender == 1)
                    a += "F_";
                else
                    a += "M_";

                if (Age >= 60)
                    a += "OLD_";
                else if (Age > 40 & Age < 60)
                    a += "MID_";
                else
                    a += "YOUNG_";

                a += $"{portraitBaseId}.png";
                return a;
            }
        }
        public List<string> AvalibaleSkills { get; set; }
        public void SetAvSkills()
        {
            var answ = new List<string>()
                {
                    "ACTION",
                    "DRAMA",
                    "HISTORICAL",
                    "THRILLER",
                    "ROMANCE",
                    "DETECTIVE",
                    "COMEDY",
                    "ADVENTURE"
                };
            switch (professions.GetProfession)
            {
                case Professions.Profession.Scriptwriter:
                case Professions.Profession.Producer:
                    break;
                case Professions.Profession.Cinematographer:
                    answ = new List<string>() { "INDOOR", "OUTDOOR" };
                    break;
                case Professions.Profession.Director:
                case Professions.Profession.Actor:
                    answ.Add("COM");
                    answ.Add("ART");
                    break;
                default:
                    AvalibaleSkills = new List<string>();
                    return;
            }
            foreach (var item in whiteTagsNEW.Select(t => t.id))
            {
                if (answ.Any(t => t == item))
                    answ.Remove(item);
            }
            AvalibaleSkills = answ;
        }
        public List<string> AvalibaleTraits { get; set; }
        public void SetAvTraits()
        {
            var answ = Character.Labels;
            switch (professions.GetProfession)
            {
                case Professions.Profession.Scriptwriter:
                case Professions.Profession.Producer:
                case Professions.Profession.FilmEditor:
                case Professions.Profession.Director:
                case Professions.Profession.Composer:
                case Professions.Profession.Cinematographer:
                case Professions.Profession.Actor:
                    break;
                default:
                    AvalibaleTraits = new List<string>();
                    return;
            }
            foreach (var item in labels)
            {
                if (answ.Any(t => t == item))
                    answ.Remove(item);
            }
            AvalibaleTraits = answ;
        }
        #endregion

        public static bool operator ==(Character a, Character b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            bool whtg = false;
            if (b.whiteTagsNEW == null) whtg = a.whiteTagsNEW == null;
            else
            if (a.whiteTagsNEW == null) whtg = b.whiteTagsNEW == null;
            else whtg = b.whiteTagsNEW.SequenceEqual(a.whiteTagsNEW);

            bool astg = false;
            if (b.aSins == null) astg = a.aSins == null;
            else
            if (a.aSins == null) astg = b.aSins == null;
            else astg = b.aSins.SequenceEqual(a.aSins);

            bool lbtg = false;
            if (b.labels == null) lbtg = a.labels == null;
            else
            if (a.labels == null) lbtg = b.labels == null;
            else lbtg = b.labels.SequenceEqual(a.labels);

            return
                b.limit == a.limit &&
                b.mood == a.mood &&
                b.attitude == a.attitude &&
                b.id == a.id &&
                b.deathDate == a.deathDate &&
                b.causeOfDeath == a.causeOfDeath &&
                b.firstNameId == a.firstNameId &&
                b.lastNameId == a.lastNameId &&
                b.birthDate == a.birthDate &&
                b.gender == a.gender &&
                b.studioId == a.studioId &&
                ((b.contract == null && a.contract == null) || (b.contract != null && a.contract != null && b.contract == a.contract)) &&
                b.professions == a.professions &&
                b.state == a.state &&
                whtg && lbtg && astg;
        }

        public static bool operator !=(Character a, Character b) => !(a == b);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return this == (Character)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + limit.GetHashCode();
                hash = hash * 23 + mood.GetHashCode();
                hash = hash * 23 + attitude.GetHashCode();
                hash = hash * 23 + id.GetHashCode();
                hash = hash * 23 + (deathDate != null ? deathDate.GetHashCode() : 0);
                hash = hash * 23 + causeOfDeath.GetHashCode();
                hash = hash * 23 + (firstNameId != null ? firstNameId.GetHashCode() : 0);
                hash = hash * 23 + (lastNameId != null ? lastNameId.GetHashCode() : 0);
                hash = hash * 23 + (birthDate != null ? birthDate.GetHashCode() : 0);
                hash = hash * 23 + gender.GetHashCode();
                hash = hash * 23 + (studioId != null ? studioId.GetHashCode() : 0);
                hash = hash * 23 + (contract != null ? contract.GetHashCode() : 0);
                hash = hash * 23 + (professions != null ? professions.GetHashCode() : 0);
                hash = hash * 23 + state.GetHashCode();
                return hash;
            }
        }

        public bool WasChanged(DateTime Now)
        {
            Character backup = BuildCharacter(JToken.Parse(JsonString), Now);
            return !(this == backup);

        }
        public override string ToString()
        {
            return $"{MyCustomName} {professions.Name}";
        }
        public static Character BuildCharacter(JToken json, DateTime Now)
        {
            Character z = JsonConvert.DeserializeObject<Character>(json.ToString());
            if (z != null)
            {
                // Оптимизиация под версию 0.8.53EA, так как образовались два поля "limit" и "Limit"
                var limitToken = json.SelectToken("limit") ?? json.SelectToken("Limit");
                if (limitToken != null)
                {
                    z.limit = limitToken.Value<double>();
                }

                z.isDead = z.deathDate != "01-01-0001";
                z.ReservDateOfDeath = z.IsDead ? z.deathDate : Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                z.ReservCauseOfDeath = z.causeOfDeath;
                z.ReservState = z.state;
                var aopm = json.SelectToken("activeOrPlannedMovies");
                if (aopm != null && aopm.HasValues)
                {
                    z.IsBusyOnJob = true;
                }
                var prof_tkn = json.SelectToken("professions").ToObject<JObject>().Properties().ElementAt(0);
                var q_prop = prof_tkn.Name;
                var q_val = prof_tkn.Value.ToObject<double>();
                z.professions = new Professions() { Name = q_prop, Value = q_val };
                z.JsonString = json.ToString();
                z.SetFullAge(Now);
                if (z.contract != null)
                {
                    z.contract.dateOfNow = Now;
                    z.contract.SetCalcDaysLeft();
                    z.contract.IsInit = false;
                }
                var tags = json.SelectToken("whiteTagsNEW");
                if (tags != null && tags.Children().Count() > 0)
                {
                    z.whiteTagsNEW = new ObservableCollection<WhiteTag>();
                    foreach (var tag in tags.Children())
                    {
                        WhiteTag whiteTag = new WhiteTag();
                        var in_tag = tag.First();
                        whiteTag.id = in_tag.SelectToken("id")?.Value<string>();
                        if (whiteTag.Tagtype == Skills.ELSE)
                            continue;
                        whiteTag.dateAdded = (DateTime)in_tag.SelectToken("dateAdded")?.Value<DateTime>();
                        whiteTag.movieId = (int)in_tag.SelectToken("movieId")?.Value<int>();
                        whiteTag.Value = (double)in_tag.SelectToken("value")?.Value<double>();
                        whiteTag.IsOverall = (bool)in_tag.SelectToken("IsOverall")?.Value<bool>();
                        whiteTag.overallValues = JsonConvert.DeserializeObject<List<OverallValue>>(in_tag.SelectToken("overallValues").ToString());
                        z.whiteTagsNEW.Add(whiteTag);
                    }
                }
                z.SetAvSkills();
                z.SetAvTraits();
                z.IsInit = false;
                return z;
            }
            return null;
        }
    }
}