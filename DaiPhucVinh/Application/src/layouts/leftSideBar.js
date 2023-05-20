import * as React from "react";
import SimpleBar from "simplebar-react";
import { Link } from "react-router-dom";
import { SiteMap } from "./siteMap";
import { decrypt } from "../framework/encrypt";

export default class LeftSidebar extends React.Component {
  state = {
    UserPermissions: [],
    newSiteMapData: [],
  };
  componentDidMount() {
    //#region Load UserPermissions
    let permissions = localStorage.getItem("@permissions");
    this.setState({ UserPermissions: decrypt(permissions) });
    //#endregion

    let newSiteMaps = _.forEach(SiteMap, (maps) => {
      _.forEach(maps.tabs, (data) => {
        _.forEach(data.tabs, (zdata) => {
          if (decrypt(permissions) && decrypt(permissions).length > 0) {
            if (decrypt(permissions).includes(`${zdata.role}`)) {
              data.isShow = true;
            }
          }
        });
      });
    });
    this.setState({ newSiteMapData: newSiteMaps });
  }

  componentDidUpdate() {
    $("#sidebar-menu").metisMenu();
    $("ul.sub-menu > li > a").on("click", autoCloseNavigator);
    function autoCloseNavigator() {
      if ($(window).width() < 992 && $("body").hasClass("sidebar-enable")) {
        $("body").removeClass("sidebar-enable");
      }
    }
  }

  render() {
    const { UserPermissions, newSiteMapData } = this.state;
    return (
      <div
        className="vertical-menu"
        style={{
          top: "35px",
        }}
      >
        <SimpleBar className="h-100">
          <div id="sidebar-menu">
            <ul className="metismenu list-unstyled" id="side-menu">
              {newSiteMapData.map((site, idx) => (
                <React.Fragment key={"site_" + idx}>
                  {site.groupName && (
                    <li className="menu-title">{site.groupName}</li>
                  )}
                  {site.tabs.map((tab, tid) =>
                    UserPermissions != undefined &&
                    UserPermissions.length > 0 &&
                    !UserPermissions.includes("ALLROLES") ? (
                      tab.href ? (
                        <React.Fragment key={"site_tab_" + tid}>
                          {tab.isShow && (
                            <li>
                              <Link
                                to={tab.href}
                                className="waves-effect"
                                style={{
                                  fontSize: "12px",
                                }}
                              >
                                {tab.icon}
                                <span
                                  style={{
                                    fontSize: "12px",
                                  }}
                                >
                                  {tab.name}
                                </span>
                              </Link>
                            </li>
                          )}
                        </React.Fragment>
                      ) : (
                        <React.Fragment key={"site_tab_" + tid}>
                          {tab.isShow && (
                            <li>
                              <a
                                href="#"
                                className="has-arrow waves-effect"
                                style={{
                                  fontSize: "12px",
                                }}
                              >
                                {tab.icon}
                                <span
                                  key="t-dashboards"
                                  style={{
                                    fontSize: "12px",
                                  }}
                                >
                                  {tab.name}
                                </span>
                              </a>
                              <ul className="sub-menu" aria-expanded="false">
                                {tab.tabs.map((ztab, zt) => (
                                  <li key={"ztab_" + zt}>
                                    {UserPermissions != null &&
                                      UserPermissions.length > 0 &&
                                      UserPermissions.includes(
                                        `${ztab?.role}`
                                      ) && (
                                        <Link
                                          to={ztab.href}
                                          key="t-tui-calendar"
                                          style={{
                                            fontSize: "12px",
                                          }}
                                        >
                                          {ztab.name}
                                        </Link>
                                      )}
                                  </li>
                                ))}
                              </ul>
                            </li>
                          )}
                        </React.Fragment>
                      )
                    ) : tab.href ? (
                      <React.Fragment key={"site_tab_" + tid}>
                        <li>
                          <Link
                            to={tab.href}
                            className="waves-effect"
                            style={{
                              fontSize: "12px",
                            }}
                          >
                            {tab.icon}
                            <span
                              style={{
                                fontSize: "12px",
                              }}
                            >
                              {tab.name}
                            </span>
                          </Link>
                        </li>
                      </React.Fragment>
                    ) : (
                      <React.Fragment key={"site_tab_" + tid}>
                        <li>
                          <a
                            href="#"
                            className="has-arrow waves-effect"
                            style={{
                              fontSize: "12px",
                            }}
                          >
                            {tab.icon}
                            <span
                              key="t-dashboards"
                              style={{
                                fontSize: "12px",
                              }}
                            >
                              {tab.name}
                            </span>
                          </a>
                          <ul className="sub-menu" aria-expanded="false">
                            {tab.tabs.map((ztab, zt) => (
                              <li key={"ztab_" + zt}>
                                <Link
                                  to={ztab.href}
                                  key="t-tui-calendar"
                                  style={{
                                    fontSize: "12px",
                                  }}
                                >
                                  {ztab.name}
                                </Link>
                              </li>
                            ))}
                          </ul>
                        </li>
                      </React.Fragment>
                    )
                  )}
                </React.Fragment>
              ))}
            </ul>
          </div>
        </SimpleBar>
      </div>
    );
  }
}
