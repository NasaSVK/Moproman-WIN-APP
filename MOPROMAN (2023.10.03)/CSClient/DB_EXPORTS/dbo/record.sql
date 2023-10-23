create table record
(
    id            int identity
        constraint PK_records
            primary key,
    pec_id        varchar(6)                    not null,
    napatie       real,
    prud          real,
    sobert_vstup  real,
    t_voda_vstup  real,
    t_voda_vystup real,
    vykon         real,
    rz_pribenie   real,
    date_time     datetime                      not null,
    sobert_vykon  real,
    tlak          real,
    zmena         varchar(10) default 'ZMENA-1' not null
)
go

