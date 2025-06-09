create database bd_pagapoco_web;

use bd_pagapoco_web;

create table tbl_user(
	id int not null auto_increment,
    first_name varchar(45) not null,
    last_name varchar(45),
    email varchar(80) not null,
    phone varchar(45),
    password varchar(45) not null,
    primary key(id)
);



/*
tabla pieza, moto, auto
*/



/*create table tbl_publications(
	id int not null auto_increment,
    title varchar(45) not null,
    decription varchar(45),
    price double not null,
    id_category int not null,
    primary key(id),
    constraint fk_category_publications foreign key(id_category) references tbl_category(id)
);*/

CREATE TABLE publicaciones (
    publication_id INT IDENTITY(1,1) PRIMARY KEY,
    type VARCHAR(20) NOT NULL,

    -- Campos comunes a todos
    brand VARCHAR(45) NOT NULL,
    model VARCHAR(45) NOT NULL,
    description VARCHAR(750),
    price MONEY NOT NULL,

    -- Comunes a vehiculos y motos (NO PERMITEN NULL)
    fabrication_year INT NOT NULL CHECK (year BETWEEN 1800 AND YEAR(GETDATE())), 
    kilometrage INT NOT NULL,
    color VARCHAR(20) NOT NULL,
    fuel_type VARCHAR(20) NOT NULL,
    transmission_system VARCHAR(25) NOT NULL,
    displacement_engine INT NOT NULL,

    -- Campos especificos de vehiculos (NO PERMITEN NULL)
    model_version VARCHAR(50) NOT NULL,
    num_door INT NOT NULL, 

    -- Campos especificos de motos
    wheeled DECIMAL(5,2),
    power DECIMAL(5,2),

    -- Campos especificos de piezas
    compatibility VARCHAR(250),
    component_status VARCHAR(15) NOT NULL
);






