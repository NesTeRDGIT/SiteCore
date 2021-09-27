import React, { Component, useEffect, useRef } from "react";
import { makeStyles } from '@material-ui/core/styles';

import Grid from '@material-ui/core/Grid';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { format } from "date-fns";

const ListItemStyles = makeStyles((theme) => ({
    red: {
        color: 'red !important'
    },
    green: {
        color: 'green !important'
    }
}));


export default function CertList(props) {
    const { items } = props;

    const classes = ListItemStyles();

    const [selectedIndex, setselectedIndex] = React.useState(0);
    const handleListItemClick = (event, index) => {
        setselectedIndex(index);
    };
    return (
        <div>
            <Grid container spacing={1}>
                <Grid item xs>
                    <div className="BoldText">Список сертификатов</div>
                    <List aria-label="secondary mailbox folder">
                        {items.map((value, index) => <ListItem className={value.VALID ? classes.green : classes.red} selected={selectedIndex === index} button onClick={(event) => handleListItemClick(event, index)}><ListItemText primary={`Сертификат:${value.CAPTION}`} /></ListItem>)}
                    </List>
                </Grid>
                <Grid item xs>
                    <ShowInfoCert data={items[selectedIndex]} />
                </Grid>
            </Grid>
        </div>
    );
}

function ShowInfoCert(props) {
    const { data } = props;
    return (
        data != null ?
            <div>
                <div className="BoldText">Информация о сертификате</div>
                <ul className="ErrorLi">{data.Error.map((err) => <li>{err}</li>)}</ul>
                <table className="table_report">
                    <tr><th>Параметр</th><th>Значение</th></tr>
                    {data.NAME != null ? <tr><td>Общее имя(CN)</td><td>{data.NAME}</td></tr> : null}
                    {data.ORG != null ? <tr><td>Организация(O)</td><td>{data.ORG}</td></tr> : null}
                    {data.PODR != null ? <tr><td>Подразделение(OU)</td><td>{data.PODR}</td></tr> : null}
                    {data.DOLG != null ? <tr><td>Должность(T)</td><td>{data.DOLG}</td></tr> : null}
                    {data.FAM != null || data.IM_OT != null ? <tr><td>ФИО(SN+GN)</td><td>{data.FAM} {data.IM_OT}</td></tr> : null}
                    {data.Country != null ? <tr><td>Страна(C)</td><td>{data.Country}</td></tr> : null}
                    {data.Region != null ? <tr><td>Регион(S)</td><td>{data.Region}</td></tr> : null}
                    {data.City != null ? <tr><td>Населенный пункт(L)</td><td>{data.City}</td></tr> : null}
                    {data.Adres != null ? <tr><td>Адрес(Street)</td><td>{data.Adres}</td></tr> : null}
                    {data.EMAIL != null ? <tr><td>Email(E)</td><td>{data.EMAIL}</td></tr> : null}
                    {data.SNILS != null ? <tr><td> СНИЛС(SNILS)</td><td>{data.SNILS}</td></tr> : null}
                    {data.INN != null ? <tr><td>ИНН(INN)</td><td>{data.INN}</td></tr> : null}
                    {data.OGRN != null ? <tr><td>ОГРН(OGRN)</td><td>{data.OGRN}</td></tr> : null}
                    <tr><td>Дата начала</td><td>{format(new Date(data.DATE_B), "dd-MM-yyyy")}</td></tr>
                    <tr><td>Дата окончания</td><td>{format(new Date(data.DATE_E), "dd-MM-yyyy")}</td></tr>

                    {data.OtherAttribute.map((value) => <tr><td>{value.Name}</td><td>{value.Value}</td></tr>)}
                </table>
            </div>
            : <div></div>
    );
}
