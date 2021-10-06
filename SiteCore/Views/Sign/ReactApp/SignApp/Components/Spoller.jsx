import React, { Component, useRef, useEffect } from "react"
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Collapse from '@material-ui/core/Collapse';

export default function Spoller(props) {

    const { caption} = props;
    const [isOpen, setIsOpen] = React.useState(false);


    const handleClick = () => {
        setIsOpen(!isOpen);
    };

    return (
        <div style={{ marginLeft: "20px" }}>
            <ListItem button onClick={handleClick}>
                <ListItemText primary={caption}  />
                {isOpen ? <ExpandLess/> : <ExpandMore/>}
            </ListItem>
            <hr/>
            <Collapse in={isOpen} timeout="auto" unmountOnExit>
               {props.children}
            </Collapse>
        </div>
    );
}