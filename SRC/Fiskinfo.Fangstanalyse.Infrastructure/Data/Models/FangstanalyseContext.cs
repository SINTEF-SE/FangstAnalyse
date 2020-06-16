using System;
using Fiskinfo.Fangstanalyse.API.ViewModels;
using Fiskinfo.Fangstanalyse.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fiskinfo.Fangstanalyse.Infrastructure
{
    public partial class FangstanalyseContext : DbContext
    {
        public FangstanalyseContext()
        {
        }

        public FangstanalyseContext(DbContextOptions<FangstanalyseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FangstDataRaw> FangstDataRaw { get; set; }
        public virtual DbSet<InteroperableCatchNote> InteroperableCatchNotes { get; set; }
        public virtual DbSet<OptimizedCatchData> OptimizedCatchData { get; set; }
        public virtual DbSet<DetailedCatchData> DetailedCatchData { get; set; }
        public virtual DbSet<WindDatum> WindData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FangstDataRaw>(entity =>
            {
                entity.ToTable("fangst_data_raw");

                entity.HasIndex(e => e.Index)
                    .HasName("ix_fangst_data_raw_index");

                entity.Property(e => e.AntallStykk).HasColumnName("antall_stykk");

                entity.Property(e => e.Anvendelse).HasColumnName("anvendelse");

                entity.Property(e => e.AnvendelseHovedgruppe).HasColumnName("anvendelse_hovedgruppe");

                entity.Property(e => e.AnvendelseHovedgruppeKode).HasColumnName("anvendelse_hovedgruppe_kode");

                entity.Property(e => e.AnvendelseKode).HasColumnName("anvendelse_kode");

                entity.Property(e => e.Art).HasColumnName("art");

                entity.Property(e => e.ArtFao).HasColumnName("art_fao");

                entity.Property(e => e.ArtFaoKode).HasColumnName("art_fao_kode");

                entity.Property(e => e.ArtKode).HasColumnName("art_kode");

                entity.Property(e => e.Artfdir).HasColumnName("artfdir");

                entity.Property(e => e.ArtfdirKode).HasColumnName("artfdir_kode");

                entity.Property(e => e.Artgruppe).HasColumnName("artgruppe");

                entity.Property(e => e.ArtgruppeKode).HasColumnName("artgruppe_kode");

                entity.Property(e => e.Arthovedgruppe).HasColumnName("arthovedgruppe");

                entity.Property(e => e.ArthovedgruppeKode).HasColumnName("arthovedgruppe_kode");

                entity.Property(e => e.BeløpForFisker).HasColumnName("beløp_for_fisker");

                entity.Property(e => e.BeløpForKjøper).HasColumnName("beløp_for_kjøper");

                entity.Property(e => e.Besetning).HasColumnName("besetning");

                entity.Property(e => e.Bruttotonnasje1969).HasColumnName("bruttotonnasje_1969");

                entity.Property(e => e.BruttotonnasjeAnnen).HasColumnName("bruttotonnasje_annen");

                entity.Property(e => e.Bruttovekt).HasColumnName("bruttovekt");

                entity.Property(e => e.Byggeår).HasColumnName("byggeår");

                entity.Property(e => e.DellandingSignal).HasColumnName("dellanding_signal");

                entity.Property(e => e.DokumentSalgsdato).HasColumnName("dokument_salgsdato");

                entity.Property(e => e.DokumentVersjonsnummer).HasColumnName("dokument_versjonsnummer");

                entity.Property(e => e.DokumentVersjonstidspunkt).HasColumnName("dokument_versjonstidspunkt");

                entity.Property(e => e.Dokumentnummer).HasColumnName("dokumentnummer");

                entity.Property(e => e.Dokumenttype).HasColumnName("dokumenttype");

                entity.Property(e => e.DokumenttypeKode).HasColumnName("dokumenttype_kode");

                entity.Property(e => e.EnhetsprisForFisker).HasColumnName("enhetspris_for_fisker");

                entity.Property(e => e.EnhetsprisForKjøper).HasColumnName("enhetspris_for_kjøper");

                entity.Property(e => e.Etterbetaling).HasColumnName("etterbetaling");

                entity.Property(e => e.FangstdagbokNummer).HasColumnName("fangstdagbok_nummer");

                entity.Property(e => e.FangstdagbokTurnummer).HasColumnName("fangstdagbok_turnummer");

                entity.Property(e => e.FangstfeltKode).HasColumnName("fangstfelt_kode");

                entity.Property(e => e.Fangstverdi).HasColumnName("fangstverdi");

                entity.Property(e => e.Fangstår).HasColumnName("fangstår");

                entity.Property(e => e.FartøyId).HasColumnName("fartøy_id");

                entity.Property(e => e.Fartøyfylke).HasColumnName("fartøyfylke");

                entity.Property(e => e.FartøyfylkeKode).HasColumnName("fartøyfylke_kode");

                entity.Property(e => e.Fartøykommune).HasColumnName("fartøykommune");

                entity.Property(e => e.FartøykommuneKode).HasColumnName("fartøykommune_kode");

                entity.Property(e => e.Fartøynasjonalitet).HasColumnName("fartøynasjonalitet");

                entity.Property(e => e.FartøynasjonalitetGruppe).HasColumnName("fartøynasjonalitet_gruppe");

                entity.Property(e => e.FartøynasjonalitetKode).HasColumnName("fartøynasjonalitet_kode");

                entity.Property(e => e.Fartøynavn).HasColumnName("fartøynavn");

                entity.Property(e => e.Fartøytype).HasColumnName("fartøytype");

                entity.Property(e => e.FartøytypeKode).HasColumnName("fartøytype_kode");

                entity.Property(e => e.FiskerId).HasColumnName("fisker_id");

                entity.Property(e => e.Fiskerkommune).HasColumnName("fiskerkommune");

                entity.Property(e => e.FiskerkommuneKode).HasColumnName("fiskerkommune_kode");

                entity.Property(e => e.Fiskernasjonalitet).HasColumnName("fiskernasjonalitet");

                entity.Property(e => e.FiskernasjonalitetKode).HasColumnName("fiskernasjonalitet_kode");

                entity.Property(e => e.ForrigeMottakstasjon).HasColumnName("forrige_mottakstasjon");

                entity.Property(e => e.Hovedområde).HasColumnName("hovedområde");

                entity.Property(e => e.HovedområdeFao).HasColumnName("hovedområde_fao");

                entity.Property(e => e.HovedområdeFaoKode).HasColumnName("hovedområde_fao_kode");

                entity.Property(e => e.HovedområdeKode).HasColumnName("hovedområde_kode");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.InndraddFangstverdi).HasColumnName("inndradd_fangstverdi");

                entity.Property(e => e.Konserveringsmåte).HasColumnName("konserveringsmåte");

                entity.Property(e => e.KonserveringsmåteKode).HasColumnName("konserveringsmåte_kode");

                entity.Property(e => e.Kvalitet).HasColumnName("kvalitet");

                entity.Property(e => e.KvalitetKode).HasColumnName("kvalitet_kode");

                entity.Property(e => e.KvotefartøyRegMerke).HasColumnName("kvotefartøy_reg.merke");

                entity.Property(e => e.Kvotetype).HasColumnName("kvotetype");

                entity.Property(e => e.KvotetypeKode).HasColumnName("kvotetype_kode");

                entity.Property(e => e.KystHavKode).HasColumnName("kyst/hav_kode");

                entity.Property(e => e.Lagsavgift).HasColumnName("lagsavgift");

                entity.Property(e => e.Landingsdato).HasColumnName("landingsdato");

                entity.Property(e => e.Landingsfylke).HasColumnName("landingsfylke");

                entity.Property(e => e.LandingsfylkeKode).HasColumnName("landingsfylke_kode");

                entity.Property(e => e.Landingsklokkeslett).HasColumnName("landingsklokkeslett");

                entity.Property(e => e.Landingskommune).HasColumnName("landingskommune");

                entity.Property(e => e.LandingskommuneKode).HasColumnName("landingskommune_kode");

                entity.Property(e => e.Landingsmåned).HasColumnName("landingsmåned");

                entity.Property(e => e.LandingsmånedKode).HasColumnName("landingsmåned_kode");

                entity.Property(e => e.Landingsmåte).HasColumnName("landingsmåte");

                entity.Property(e => e.LandingsmåteKode).HasColumnName("landingsmåte_kode");

                entity.Property(e => e.Landingsnasjon).HasColumnName("landingsnasjon");

                entity.Property(e => e.LandingsnasjonKode).HasColumnName("landingsnasjon_kode");

                entity.Property(e => e.Landingstidspunkt).HasColumnName("landingstidspunkt");

                entity.Property(e => e.LatHovedområde).HasColumnName("lat_hovedområde");

                entity.Property(e => e.LatLokasjon).HasColumnName("lat_lokasjon");

                entity.Property(e => e.Lengdegruppe).HasColumnName("lengdegruppe");

                entity.Property(e => e.LengdegruppeKode).HasColumnName("lengdegruppe_kode");

                entity.Property(e => e.Linjenummer).HasColumnName("linjenummer");

                entity.Property(e => e.Lok).HasColumnName("lok");

                entity.Property(e => e.LokasjonKode).HasColumnName("lokasjon_kode");

                entity.Property(e => e.LonHovedområde).HasColumnName("lon_hovedområde");

                entity.Property(e => e.LonLokasjon).HasColumnName("lon_lokasjon");

                entity.Property(e => e.Motorbyggeår).HasColumnName("motorbyggeår");

                entity.Property(e => e.Motorkraft).HasColumnName("motorkraft");

                entity.Property(e => e.MottakendeFartNasj).HasColumnName("mottakende_fart.nasj");

                entity.Property(e => e.MottakendeFartType).HasColumnName("mottakende_fart.type");

                entity.Property(e => e.MottakendeFartøyRegMerke).HasColumnName("mottakende_fartøy_reg.merke");

                entity.Property(e => e.MottakendeFartøyRkal).HasColumnName("mottakende_fartøy_rkal");

                entity.Property(e => e.MottakendeFartøynasjKode).HasColumnName("mottakende_fartøynasj._kode");

                entity.Property(e => e.MottakendeFartøytypeKode).HasColumnName("mottakende_fartøytype_kode");

                entity.Property(e => e.MottakerId).HasColumnName("mottaker_id");

                entity.Property(e => e.Mottakernasjonalitet).HasColumnName("mottakernasjonalitet");

                entity.Property(e => e.MottakernasjonalitetKode).HasColumnName("mottakernasjonalitet_kode");

                entity.Property(e => e.Mottaksstasjon).HasColumnName("mottaksstasjon");

                entity.Property(e => e.NesteMottaksstasjon).HasColumnName("neste_mottaksstasjon");

                entity.Property(e => e.NordSørFor62GraderNord).HasColumnName("nord/sør_for_62_grader_nord");

                entity.Property(e => e.Ombyggingsår).HasColumnName("ombyggingsår");

                entity.Property(e => e.Områdegruppering).HasColumnName("områdegruppering");

                entity.Property(e => e.Oppdateringstidspunkt).HasColumnName("oppdateringstidspunkt");

                entity.Property(e => e.Produksjonsanlegg).HasColumnName("produksjonsanlegg");

                entity.Property(e => e.Produksjonskommune).HasColumnName("produksjonskommune");

                entity.Property(e => e.ProduksjonskommuneKode).HasColumnName("produksjonskommune_kode");

                entity.Property(e => e.Produkttilstand).HasColumnName("produkttilstand");

                entity.Property(e => e.ProdukttilstandKode).HasColumnName("produkttilstand_kode");

                entity.Property(e => e.Produktvekt).HasColumnName("produktvekt");

                entity.Property(e => e.ProduktvektOverKvote).HasColumnName("produktvekt_over_kvote");

                entity.Property(e => e.RadiokallesignalSeddel).HasColumnName("radiokallesignal_seddel");

                entity.Property(e => e.Redskap).HasColumnName("redskap");

                entity.Property(e => e.RedskapKode).HasColumnName("redskap_kode");

                entity.Property(e => e.Redskapgruppe).HasColumnName("redskapgruppe");

                entity.Property(e => e.RedskapgruppeKode).HasColumnName("redskapgruppe_kode");

                entity.Property(e => e.Redskaphovedgruppe).HasColumnName("redskaphovedgruppe");

                entity.Property(e => e.RedskaphovedgruppeKode).HasColumnName("redskaphovedgruppe_kode");

                entity.Property(e => e.RegistreringsmerkeSeddel).HasColumnName("registreringsmerke_seddel");

                entity.Property(e => e.Rundvekt).HasColumnName("rundvekt");

                entity.Property(e => e.RundvektOverKvote).HasColumnName("rundvekt_over_kvote");

                entity.Property(e => e.Salgslag).HasColumnName("salgslag");

                entity.Property(e => e.SalgslagId).HasColumnName("salgslag_id");

                entity.Property(e => e.SalgslagKode).HasColumnName("salgslag_kode");

                entity.Property(e => e.SisteFangstdato).HasColumnName("siste_fangstdato");

                entity.Property(e => e.Sone).HasColumnName("sone");

                entity.Property(e => e.SoneKode).HasColumnName("sone_kode");

                entity.Property(e => e.StørrelsesgrupperingKode).HasColumnName("størrelsesgruppering_kode");

                entity.Property(e => e.StørsteLengde).HasColumnName("største_lengde");

                entity.Property(e => e.Støttebeløp).HasColumnName("støttebeløp");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}