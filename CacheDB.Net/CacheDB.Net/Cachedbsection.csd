<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="b32d43aa-cc38-43e3-aacc-559d689b40c1" namespace="CacheDB.Net" xmlSchemaNamespace="urn:CacheDB.Net" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="CacheDbSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="cacheDbSection">
      <elementProperties>
        <elementProperty name="Levels" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="levels" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/LevelCachedCollection" />
          </type>
        </elementProperty>
        <elementProperty name="DbManager" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="dbManager" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/DbManagerConfig" />
          </type>
        </elementProperty>
        <elementProperty name="Policies" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="policies" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/PolicyCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="LevelCachedConfig">
      <attributeProperties>
        <attributeProperty name="UpgradeValue" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="upgradeValue" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Maximum" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="maximum" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="LevelCachedCollection" collectionType="AddRemoveClearMap" xmlItemName="levelCachedConfig" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/LevelCachedConfig" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="DbManagerConfig">
      <attributeProperties>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Formater" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="formater" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Properties" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="properties" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/PropertyCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="PropertyConfig">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Value" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="PropertyCollection" collectionType="AddRemoveClearMap" xmlItemName="propertyConfig" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/PropertyConfig" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="PolicyConfig">
      <attributeProperties>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ObjectType" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="objectType" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Properties" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="properties" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/PropertyCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="PolicyCollection" collectionType="AddRemoveClearMap" xmlItemName="policyConfig" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/b32d43aa-cc38-43e3-aacc-559d689b40c1/PolicyConfig" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>