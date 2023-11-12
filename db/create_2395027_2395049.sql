drop schema if exists car_rental cascade;
create schema if not exists car_rental;
set search_path to car_rental;



create table category
(
    category_id integer generated always as identity
        primary key,
    type        text
);


create table car
(
    car_id       integer generated always as identity primary key,
    brand        text    not null,
    model        text    not null,
    trim         text    not null,
    year         integer not null
        check ( year >= '1900' and year <= extract(year from current_date)),
    transmission text    not null default 'automatic'
        check (transmission in ('manual', 'auto')),                             -- Automatic, Manual, CVT
    fuel_type    text    not null default 'gasoline'
        check (fuel_type in ('electric', 'gasoline', 'hybird') ),               -- electric, Gasoline
    body_type    text    not null default 'sedan'
        check (body_type in ('suv', 'sedan', 'truck', 'minivan', 'hatchback', 'coupe',
                             'other') ),                                        -- suv,sedan,truck,minivan,hatchback,coupe
    seats        integer not null default '4'
        check ( seats >= 1 and seats <= 7),
    doors        integer not null default 5
        check ( doors >= 2 and doors <= 5 ),
--     category_id  integer not null references category (category_id),

    constraint car_unique_check unique (brand, model, trim, year, transmission) -- transmission, fuel_type, body_type, seats, doors)
);

insert into car (brand, model, year, trim, transmission, fuel_type, body_type, seats, doors)
values ('Dodge', 'Durango', 2009, 'P91', 'manual', 'gasoline', 'sedan', 4, 5);
insert into car (brand, model, year, trim, transmission, fuel_type, body_type, seats, doors)
values ('Land Rover', 'Defender 90', 1994, 't9', 'manual', 'electric', 'suv', 4, 5);
insert into car (brand, model, year, trim, transmission, fuel_type, body_type, seats, doors)
values ('Lamborghini', 'Murciélago', 2009, 'Lxauto', 'auto', 'gasoline', 'sedan', 2, 2);
insert into car (brand, model, year, trim, transmission, fuel_type, body_type, seats, doors)
values ('MINI', 'Cooper Countryman', 2011, 'S3', 'auto', 'electric', 'sedan', 4, 5);

create function check_car_data() returns TRIGGER as
$$
begin

    new.body_type = lower(new.body_type);
    new.transmission = lower(new.transmission);
    new.fuel_type = lower(new.fuel_type);
    new.body_type = lower(new.body_type);
    return NEW;
end;
$$ language plpgsql;

create trigger validate_car_data
    before insert or update
    on car
    for each row
execute function check_car_data();



drop domain if exists phone_domain;
create domain phone_domain as text
    check (
        -- regexp_like(value, '^[0-9]+$')
        regexp_like(value, '^[0-9]{3}-[0-9]{3}-[0-9]{4}$')
        );


create table service_point
(
    service_id     integer generated always as identity primary key,
    address        jsonb        not null,
    contact_number phone_domain not null

    /*constraint phone_check check ( length(phone) >= 10 and length(phone) <= 13
        and regexp_like(phone, '^[0-9]+$') )*/

);

/*alter table service_point
add constraint address_postal_code_check
check ( length(address->>'Postal_code') = 6 );*/

create table inventory
(
    inventory_id     integer generated always as identity primary key,
    vin              varchar(17) unique not null
        check ( length(vin) = 17 ),
    color            text               not null,
    rent_price       money              not null,
    deposit          money              not null default '1.00',
    cost             money              not null,
    car_id           integer            not null references car (car_id)
--     service_point_id integer            not null references service_point (service_id),
--     contract_id      integer            not null --references contract(contract_id)
);

insert into inventory ( VIN, color, rent_price, cost, car_id )
values ( 'JN8AZ2KR1AT007853', 'Black', '$162.48', '$71718.19', 1);
insert into inventory ( VIN, color, rent_price, cost, car_id )
values ( 'WVCAV3AX1DW506090', 'Sliver', '$536.59', '$42731.88', 2);
insert into inventory ( VIN, color, rent_price, cost, car_id )
values ( 'WVGAV3AX1DW506020', 'Sliver', '$536.59', '$42731.88', 3);
insert into inventory (VIN, color, rent_price, cost, car_id )
values ( '1FTEW1CW4AF425095', 'Green', '$217.81', '$36951.50', 4);

create table garage
(
    garage_id integer generated always as identity primary key,
    address   jsonb not null,
    phone     text  not null,
    contact   text  not null
);

create table maintenance
(
    maintenance_id integer generated always as identity primary key,
    date           date    not null,
    description    text,
    mechanic       text,
    inventory_id   integer not null references inventory (inventory_id),
    garage_id      integer not null references garage (garage_id)
);

drop domain if exists email_domain;
create domain email_domain as text
    check (
--         regexp_like(value, '[\w.%+-]+@[\w.-]+\.[a-zA-Z]{2,6}')
        value like '%@%.%'
        );

create table insurance
(
    insurance_id integer generated always as identity primary key,
    name         text         not null,
    address      jsonb        not null,
    phone        phone_domain not null,
    email        email_domain not null

);


create table contract
(
    contract_id  integer generated by default as identity primary key,
    start_date   date    not null,
    end_date     date    not null,
    price        money   not null,
    description  text,
    inventory_id integer not null, --references inventory (inventory_id),
    insurance_id integer not null references insurance (insurance_id),

    constraint date_check check ( start_date < end_date)
);

-- alter table inventory
--     add constraint inventory_contract_fk foreign key (contract_id) references contract (contract_id) deferrable;
--
-- alter table contract
--     add constraint contract_inventory_fk foreign key (inventory_id) references inventory (inventory_id) deferrable;


create table staff_authority
(
    id        integer generated always as identity primary key,
    user_name text not null unique,
    password  text not null,
    salt      text not null,
    phone     text not null unique,
    email     text not null unique,
    authority  integer not null default 0
);

insert into staff_authority (user_name, password, salt, phone, email, authority)
values ('admin', 'admin123', 'admin_car_rental', '514-332-1234', 'admin123456', 99);



create table staff
(
    staff_id           integer generated always as identity primary key,
    first_name         text         not null,
    last_name          text         not null,
    address            jsonb,
    service_point_id   integer      not null references service_point (service_id),
    staff_authority_id integer      not null references staff_authority (id)
);

create table customer_authority
(
    id        integer generated always as identity primary key,
    user_name text not null unique,
    password  text not null,
    salt      text not null,
    phone     text not null unique,
    email     text not null unique
);

insert into customer_authority (user_name, password, salt, phone, email)
values ('Peter', 'abc123456', 'admin_car_rental', '514-001-1234', 'peter@email.com');
insert into customer_authority (user_name, password, salt, phone, email)
values ('Julie', 'abc123456', 'admin_car_rental', '514-002-1234', 'julie@email.com');



create table customer
(
    customer_id           integer generated always as identity primary key,
    first_name            text         not null,
    last_name             text         not null,
--     phone                 text null,
    driver_license        jsonb        null,
--     email                 text,
    address               jsonb        not null,
    customer_authority_id integer      not null references customer_authority (id)
);



create table reservation
(
    reservation_id   integer generated always as identity primary key,
    reservation_time timestamp(0) not null,
    delivery_time    timestamp(0) not null,
    return_time      timestamp(0) not null,
--     staff_id         integer references staff_authority (id),
    customer_id      integer      not null references customer_authority (id),
    inventory_id     integer      not null references inventory (inventory_id),
--     deposit_id       integer      not null, --references deposit(deposit_id)
    check ( return_time > delivery_time and return_time > reservation_time and delivery_time > reservation_time )
);

insert into reservation(reservation_time, delivery_time, return_time,  customer_id, inventory_id )
VALUES ('2022-06-10 12:07:19', '2022-06-11 10:00:00', '2022-06-22 10:00:00',  1, 1);
insert into reservation(reservation_time, delivery_time, return_time,  customer_id, inventory_id)
VALUES ('2022-05-20 12:07:19', '2022-05-21 10:00:00', '2022-06-22 10:00:00',  2, 2);
insert into reservation(reservation_time, delivery_time, return_time,  customer_id, inventory_id)
values('2023-11-06 01:41:53',  '2023-11-07 01:41:53', '2023-11-08 01:41:53', 1, 1);
create table payment
(
    payment_id     integer generated always as identity primary key,
    date           timestamp(0) not null,
    amount         money        not null,
    reservation_id integer      not null references reservation (reservation_id)
);

create table deposit
(
    deposit_id      integer generated by default as identity primary key,
    deposit         money   not null,
    expected_return date    not null,
    if_return       boolean not null default false,
    reservation_id  integer not null references reservation (reservation_id)
--     customer_id     integer not null references customer (customer_id)
);

-- alter table reservation
--     add constraint reservation_deposit_id foreign key (deposit_id) references deposit (deposit_id) deferrable;
--
-- alter table deposit
--     add constraint deposit_reservation_id foreign key (reservation_id) references reservation (reservation_id) deferrable;


