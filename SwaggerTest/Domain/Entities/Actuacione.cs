﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace SwaggerTest.Domain.Entities;

public partial class Actuacione
{
    public int IdActuacion { get; set; }

    public int? IdPelicula { get; set; }

    public int? IdActor { get; set; }

    public string Papel { get; set; }

    public virtual Actore IdActorNavigation { get; set; }

    public virtual Pelicula IdPeliculaNavigation { get; set; }
}