import React, { Component, useRef, useEffect } from "react"
import ReactDOM from "react-dom"
import RoleList from "./Components/RoleList.jsx"
import SIGN_LIST from "./Components/SIGN_LIST.jsx"
import IssuerList from "./Components/IssuerList.jsx"
import DOC_LIST from "./Components/DOC_LIST.jsx"



import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";
import Box from '@material-ui/core/Box';
import Typography from '@material-ui/core/Typography';
import { createTheme, ThemeProvider} from '@material-ui/core/styles';


const theme = createTheme({
    palette: {
        primary: {
            // light: will be calculated from palette.primary.main,
            main: "#4CAF50",
            // dark: will be calculated from palette.primary.main,
            // contrastText: will be calculated to contrast with palette.primary.main
        },
        secondary: {
            light: '#0066ff',
            main: '#0044ff',
            // dark: will be calculated from palette.secondary.main,
            contrastText: '#ffcc00',
        },
        // Used by `getContrastText()` to maximize the contrast between
        // the background and the text.
        tonalOffset: 0.2,
    },
});


function TabPanel(props) {
    const { children, value, index, ...other } = props;
    return (
        <div role="tabpanel" hidden={value !== index} id={`simple-tabpanel-${index}`} aria-labelledby={`simple-tab-${index}`} {...other}>
            {value === index && (
                <Box p={3}>
                    <Typography>{children}</Typography>
                </Box>
            )}
        </div>
    );
}



export function MainTab(props) {
    const { isAdmin } = props;
    const [value, setValue] = React.useState(0);

    const handleChange = (event, newValue) => {
        setValue(newValue);
    };

    return (
        <div>
            <ThemeProvider theme={theme}>

                <Tabs value={value} indicatorColor="primary" textColor="primary" onChange={handleChange} aria-label="disabled tabs example">
                    <Tab label="Документы на подпись"/>
                    {isAdmin ? <Tab label="Подписи пользователей" /> :null}
                    {isAdmin ? <Tab label="Роли подписей" /> : null}
                    {isAdmin ? <Tab label="Доверенные издатели" /> : null}


                </Tabs>
                <TabPanel value={value} index={0}>
                    <DOC_LIST isAdmin={isAdmin}/>
                </TabPanel>
                {isAdmin ? <TabPanel value={value} index={1}><SIGN_LIST /></TabPanel> : null}
                {isAdmin ? <TabPanel value={value} index={2}><RoleList /></TabPanel> : null}
                {isAdmin ? <TabPanel value={value} index={3}><IssuerList /></TabPanel> : null}
            </ThemeProvider>
        </div>
       
    );
}

var app = document.getElementById("app");
var isAdmin = app.attributes["isAdmin"].value==='True';

ReactDOM.render(<MainTab isAdmin={isAdmin}/> , app);
